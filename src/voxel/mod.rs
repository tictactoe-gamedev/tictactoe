mod component;
mod events;
mod systems;

use bevy::{
    app::{Plugin, Update},
    asset::{AssetServer, Assets, Handle},
    ecs::{
        event::{Event, EventReader, EventWriter},
        system::{Query, Res, ResMut},
    },
    input::{keyboard::KeyCode, Input},
    math::Vec3,
    pbr::StandardMaterial,
    render::{color::Color, mesh::Mesh},
    transform::components::Transform,
    utils::default,
};
use bevy_rand::{prelude::ChaCha8Rng, resource::GlobalEntropy};
use rand_core::RngCore;

use crate::config::WorldConfiguration;

// Components and events can be accessed by other modules
pub use self::{component::Voxel, events::VoxelEvents};
// Systems can't be accessed by other modules
use self::systems::{despawn_voxel::despawn_voxel, spawn_voxel::spawn_voxel};

pub const VOXEL_COLOURS: &[Color] = &[
    Color::GOLD,
    Color::SILVER,
    Color::ORANGE_RED,
    Color::SEA_GREEN,
];

pub struct VoxelPlugin;

impl Plugin for VoxelPlugin {
    fn build(&self, app: &mut bevy::prelude::App) {
        app.add_event::<VoxelEvents>()
            .add_systems(Update, spawn_voxel)
            .add_systems(Update, despawn_voxel);

        // Temporary items
        {
            app.add_systems(Update, create_voxel_on_key_press)
                .add_event::<CreateVoxelKeyboardInputDetected>()
                .add_systems(Update, get_voxel_type_and_position)
                .add_event::<CreateVoxelVerifyCollision>()
                .add_systems(Update, verify_collision_on_voxel_spawn)
                .add_event::<CreateVoxelTypeAndTransform>()
                .add_systems(Update, get_voxel_mesh)
                .add_event::<CreateVoxelMesh>()
                .add_systems(Update, get_voxel_material);
        }
    }
}

#[derive(Debug, Event)]
pub struct CreateVoxelKeyboardInputDetected;

fn create_voxel_on_key_press(
    keyboard: Res<Input<KeyCode>>,
    mut create_voxel: EventWriter<CreateVoxelKeyboardInputDetected>,
) {
    if keyboard.just_pressed(KeyCode::K) {
        create_voxel.send(CreateVoxelKeyboardInputDetected);
    }
}

#[derive(Debug, Event)]
struct CreateVoxelTypeAndTransform(u64, f32, f32);

fn get_voxel_type_and_position(
    mut entropy: ResMut<GlobalEntropy<ChaCha8Rng>>,
    world_config: Res<WorldConfiguration>,
    mut event_reader: EventReader<CreateVoxelKeyboardInputDetected>,
    mut event_writer: EventWriter<CreateVoxelTypeAndTransform>,
) {
    event_writer.send_batch(event_reader.read().map(|_| {
        let voxel_type = entropy.next_u64() % 4;
        let x =
            ((entropy.next_u32() % world_config.world_length()) as i32 + world_config.x_min) as f32;
        let z =
            ((entropy.next_u32() % world_config.world_width()) as i32 + world_config.z_min) as f32;
        CreateVoxelTypeAndTransform(voxel_type, x, z)
    }));
}

#[derive(Debug, Event)]
struct CreateVoxelVerifyCollision(u64, f32, f32);

fn verify_collision_on_voxel_spawn(
    world_config: Res<WorldConfiguration>,
    voxels: Query<(&Transform, &Voxel)>,
    mut event_reader: EventReader<CreateVoxelTypeAndTransform>,
    mut event_writer: EventWriter<CreateVoxelVerifyCollision>,
    mut event_retry: EventWriter<CreateVoxelKeyboardInputDetected>,
) {
    let voxel_count = voxels.iter().count();
    for event in event_reader.read() {
        let is_world_full = if let Ok(world_size) =
            usize::try_from(world_config.world_length() * world_config.world_width())
        {
            voxel_count >= world_size
        } else {
            bevy::log::error!("Failed to calculate world size.");
            true
        };

        if is_world_full {
            bevy::log::error!("World is already full of voxels.");
        } else {
            let CreateVoxelTypeAndTransform(voxel_type, voxel_x, voxel_z) = event;
            if voxels.iter().any(|(trans, voxel)| {
                trans
                    .translation
                    .abs_diff_eq(Vec3::from_array([*voxel_x, 0., *voxel_z]), f32::EPSILON)
                    || (voxel_type == &0 && voxel.voxel_type() == &0)
            }) {
                event_retry.send(CreateVoxelKeyboardInputDetected);
            } else {
                event_writer.send(CreateVoxelVerifyCollision(*voxel_type, *voxel_x, *voxel_z));
            }
        }
    }
}

#[derive(Debug, Event)]
struct CreateVoxelMesh(u64, f32, f32, Handle<Mesh>);

fn get_voxel_mesh(
    asset_server: Res<AssetServer>,
    mut event_reader: EventReader<CreateVoxelVerifyCollision>,
    mut event_writer: EventWriter<CreateVoxelMesh>,
) {
    event_writer.send_batch(event_reader.read().map(|event| {
        let CreateVoxelVerifyCollision(voxel_type, voxel_x, voxel_z) = event;
        let handle = asset_server.load("cube.gltf#Mesh0/Primitive0");
        CreateVoxelMesh(*voxel_type, *voxel_x, *voxel_z, handle)
    }));
}

fn get_voxel_material(
    mut standard_materials: ResMut<Assets<StandardMaterial>>,
    mut event_reader: EventReader<CreateVoxelMesh>,
    mut event_writer: EventWriter<VoxelEvents>,
) {
    event_writer.send_batch(event_reader.read().map(|event| {
        let CreateVoxelMesh(voxel_type, voxel_x, voxel_z, voxel_mesh) = event;
        let handle = standard_materials.add(StandardMaterial {
            base_color: VOXEL_COLOURS[*voxel_type as usize],
            ..default()
        });
        VoxelEvents::SpawnVoxel(*voxel_type, *voxel_x, *voxel_z, voxel_mesh.clone(), handle)
    }));
}
