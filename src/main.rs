mod config;
mod voxel;

use bevy::{
    app::{App, Startup, Update},
    asset::AssetServer,
    core_pipeline::core_3d::Camera3dBundle,
    ecs::{
        change_detection::DetectChanges,
        entity::Entity,
        event::{EventReader, EventWriter},
        query::With,
        schedule::IntoSystemConfigs,
        system::{Commands, Local, Query, Res, ResMut},
    },
    math::{Quat, Vec3},
    pbr::{AmbientLight, PointLight, PointLightBundle},
    render::color::Color,
    scene::SceneBundle,
    time::Time,
    transform::components::Transform,
    utils::default,
    DefaultPlugins,
};
#[cfg(feature = "with-inspector")]
use bevy_inspector_egui::quick::ResourceInspectorPlugin;
use bevy_rand::{plugin::EntropyPlugin, prelude::ChaCha8Rng};
use serde::Deserialize;
use voxel::SpawnVoxelEvent;

use crate::{
    config::{WorldConfiguration, WorldConfigurationChanged},
    voxel::{DespawnVoxelEvent, Voxel, VoxelPlugin},
};

#[derive(Deserialize)]
struct VoxelCraft {
    world_configuration: WorldConfiguration,
}

fn main() -> std::io::Result<()> {
    let voxelcraft = read_world_config()?;

    let mut app = App::new();

    // Plugins
    app.add_plugins(DefaultPlugins)
        .add_plugins(EntropyPlugin::<ChaCha8Rng>::default())
        .add_plugins(VoxelPlugin);

    // Resources
    app.insert_resource(voxelcraft.world_configuration);

    // Inspector feature plugins, events and systems
    #[cfg(feature = "with-inspector")]
    {
        app.add_event::<WorldConfigurationChanged>()
            .register_type::<WorldConfiguration>()
            .add_plugins(ResourceInspectorPlugin::<WorldConfiguration>::default())
            .add_systems(
                Update,
                (on_world_config_changed, cull_outside_world_bounds).chain(),
            );
    }

    // Systems
    app.add_systems(Startup, create_camera)
        .add_systems(Startup, spawn_fox)
        .add_systems(Update, spawn_voxels_on_timer)
        .run();

    Ok(())
}

fn read_world_config() -> std::io::Result<VoxelCraft> {
    let data = std::fs::read_to_string("voxelcraft.toml")?;
    let voxelcraft =
        toml::from_str(&data).map_err(|err| std::io::Error::new(std::io::ErrorKind::Other, err))?;
    Ok(voxelcraft)
}

fn create_camera(mut commands: Commands) {
    let camera_and_light_transform =
        Transform::from_xyz(-15., 15., -15.).looking_at(Vec3::ZERO, Vec3::Y);
    let light_transform = Transform::from_xyz(0., 15., 0.).looking_at(Vec3::ZERO, Vec3::Z);

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
        transform: light_transform,
        ..default()
    });
    commands.insert_resource(AmbientLight {
        color: Color::WHITE,
        brightness: 0.2,
    });
}

fn spawn_voxels_on_timer(
    time: Res<Time>,
    mut elapsed: Local<f64>,
    mut event_writer: EventWriter<SpawnVoxelEvent>,
) {
    const TIMER: f64 = 0.025;
    *elapsed += time.delta_seconds_f64();
    if *elapsed >= TIMER {
        *elapsed -= TIMER;
        event_writer.send(SpawnVoxelEvent);
    }
}

fn spawn_fox(mut commands: Commands, asset_server: Res<AssetServer>) {
    let scene = asset_server.load("Fox.gltf#Scene0");
    commands.spawn(SceneBundle {
        scene,
        transform: Transform {
            translation: Vec3::from_array([0., 1., 0.]),
            rotation: Quat::from_rotation_y(std::f32::consts::PI),
            scale: Vec3::splat(1. / 32.),
        },
        ..default()
    });
}

#[cfg(feature = "with-inspector")]
fn on_world_config_changed(
    mut world_config: ResMut<WorldConfiguration>,
    mut change_event_writer: EventWriter<WorldConfigurationChanged>,
) {
    if world_config.is_changed() {
        if world_config.x_max <= world_config.x_min {
            world_config.x_max = world_config.x_min + 1;
        }
        if world_config.z_max <= world_config.z_min {
            world_config.z_max = world_config.z_min + 1;
        }
        change_event_writer.send(WorldConfigurationChanged);
    }
}

#[cfg(feature = "with-inspector")]
fn cull_outside_world_bounds(
    world_config: Res<WorldConfiguration>,
    voxels: Query<(Entity, &Transform), With<Voxel>>,
    world_changed: EventReader<WorldConfigurationChanged>,
    mut event: EventWriter<DespawnVoxelEvent>,
) {
    if !world_changed.is_empty() {
        event.send_batch(voxels.iter().filter_map(|(ent, transf)| {
            if transf.translation.x < world_config.x_min as f32
                || transf.translation.x > world_config.x_max as f32
                || transf.translation.z < world_config.z_min as f32
                || transf.translation.z > world_config.z_max as f32
            {
                Some(DespawnVoxelEvent(ent))
            } else {
                None
            }
        }));
    }
}
