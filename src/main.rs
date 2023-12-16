mod voxel;

use bevy::{
    app::{App, Startup},
    asset::{AssetServer, Assets},
    core_pipeline::core_3d::Camera3dBundle,
    ecs::system::{Commands, Res, ResMut},
    math::Vec3,
    pbr::{AmbientLight, PbrBundle, PointLight, PointLightBundle, StandardMaterial},
    render::color::Color,
    transform::components::Transform,
    utils::default,
    DefaultPlugins,
};
use bevy_rand::{plugin::EntropyPlugin, prelude::ChaCha8Rng, resource::GlobalEntropy};
use rand_core::RngCore;
use voxel::Voxel;

fn main() {
    App::new()
        .add_plugins(DefaultPlugins)
        .add_plugins(EntropyPlugin::<ChaCha8Rng>::default())
        .add_systems(Startup, create_camera)
        .add_systems(Startup, create_voxel)
        .run();
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
    asset_server: Res<AssetServer>,
) {
    const COLOURS: &[Color] = &[
        Color::GOLD,
        Color::SILVER,
        Color::ORANGE_RED,
        Color::SEA_GREEN,
    ];
    let voxel_type = entropy.next_u64() % 4;
    let x = (entropy.next_u32() % 20) as i32 - 10;
    let z = (entropy.next_u32() % 20) as i32 - 10;

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
