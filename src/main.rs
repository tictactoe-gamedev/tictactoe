mod voxel;

use bevy::{
    app::{App, Startup, Update},
    asset::{AssetServer, Assets},
    core_pipeline::core_3d::Camera3dBundle,
    ecs::{
        change_detection::DetectChanges,
        reflect::ReflectResource,
        system::{Commands, Res, ResMut, Resource, RunSystemOnce},
        world::World,
    },
    math::Vec3,
    pbr::{AmbientLight, PbrBundle, PointLight, PointLightBundle, StandardMaterial},
    reflect::Reflect,
    render::color::Color,
    transform::components::Transform,
    utils::default,
    DefaultPlugins,
};
#[cfg(feature = "with-inspector")]
use bevy_inspector_egui::{
    inspector_options::{InspectorOptions, ReflectInspectorOptions},
    quick::ResourceInspectorPlugin,
};
use bevy_rand::{plugin::EntropyPlugin, prelude::ChaCha8Rng, resource::GlobalEntropy};
use rand_core::RngCore;

use crate::voxel::Voxel;

#[derive(Debug, Resource, Reflect)]
#[reflect(Resource)]
#[cfg_attr(
    feature = "with-inspector",
    derive(InspectorOptions),
    reflect(InspectorOptions)
)]
struct WorldConfiguration {
    #[cfg_attr(
        feature = "with-inspector",
        inspector(min = -100, max = 100)
    )]
    x_min: i32,
    #[cfg_attr(
        feature = "with-inspector",
        inspector(min = -100, max = 100)
    )]
    x_max: i32,
    #[cfg_attr(
        feature = "with-inspector",
        inspector(min = -100, max = 100)
    )]
    z_min: i32,
    #[cfg_attr(
        feature = "with-inspector",
        inspector(min = -100, max = 100)
    )]
    z_max: i32,
}

impl Default for WorldConfiguration {
    fn default() -> Self {
        Self {
            x_min: -10,
            x_max: 10,
            z_min: -10,
            z_max: 10,
        }
    }
}

fn main() {
    let mut app = App::new();
    app.init_resource::<WorldConfiguration>()
        .register_type::<WorldConfiguration>()
        .add_plugins(DefaultPlugins)
        .add_plugins(EntropyPlugin::<ChaCha8Rng>::default());

    #[cfg(feature = "with-inspector")]
    {
        app.add_plugins(ResourceInspectorPlugin::<WorldConfiguration>::default())
            .add_systems(Update, on_world_config_changed);
    }

    app.add_systems(Startup, create_camera).run();
}

fn create_camera(mut commands: Commands) {
    let camera_and_light_transform =
        Transform::from_xyz(15., 15., 15.).looking_at(Vec3::ZERO, Vec3::Y);

    // Camera in 3D space.
    commands.spawn(Camera3dBundle {
        transform: camera_and_light_transform,
        ..default()
    });

    // Light up the scene.
    commands.spawn(PointLightBundle {
        point_light: PointLight {
            intensity: 1000.0,
            range: 100.0,
            ..default()
        },
        transform: camera_and_light_transform,
        ..default()
    });
    commands.insert_resource(AmbientLight {
        color: Color::WHITE,
        brightness: 0.2,
    });
}

fn create_voxel(
    mut commands: Commands,
    mut entropy: ResMut<GlobalEntropy<ChaCha8Rng>>,
    mut materials: ResMut<Assets<StandardMaterial>>,
    world_config: Res<WorldConfiguration>,
    asset_server: Res<AssetServer>,
) {
    const COLOURS: &[Color] = &[
        Color::GOLD,
        Color::SILVER,
        Color::ORANGE_RED,
        Color::SEA_GREEN,
    ];

    let voxel_type = entropy.next_u64() % 4;
    let x = (entropy.next_u32() % (world_config.x_max.abs_diff(world_config.x_min))) as i32
        + world_config.x_min;
    let z = (entropy.next_u32() % (world_config.z_max.abs_diff(world_config.z_min))) as i32
        + world_config.z_min;

    let mesh = asset_server.load("cube.gltf#Mesh0/Primitive0");

    commands.spawn((
        PbrBundle {
            mesh,
            material: materials.add(StandardMaterial {
                base_color: COLOURS[voxel_type as usize],
                ..default()
            }),
            transform: Transform {
                translation: Vec3 {
                    x: x as f32,
                    y: 0.,
                    z: z as f32,
                },
                scale: Vec3::splat(0.5),
                ..default()
            },
            ..default()
        },
        Voxel::new(voxel_type, 1),
    ));
}

fn on_world_config_changed(mut commands: Commands, mut world_config: ResMut<WorldConfiguration>) {
    if world_config.is_changed() {
        bevy::log::info!("World Config changed");
        if world_config.x_max <= world_config.x_min {
            world_config.x_max = world_config.x_min + 1;
        }
        if world_config.z_max <= world_config.z_min {
            world_config.z_max = world_config.z_min + 1;
        }
        commands.add(|world: &mut World| {
            let voxels = world
                .iter_entities()
                .filter(|ent_ref| ent_ref.contains::<Voxel>())
                .map(|ent_ref| ent_ref.id())
                .collect::<Vec<_>>();
            for voxel in voxels {
                world.despawn(voxel);
            }

            world.run_system_once(create_voxel);
        });
    }
}
