mod component;

use bevy::{
    app::{Plugin, Startup},
    asset::AssetServer,
    ecs::system::{Commands, Res},
    math::{Quat, Vec3},
    scene::SceneBundle,
    transform::components::Transform,
    utils::default,
};

use self::component::Fox;

pub struct FoxPlugin;

impl Plugin for FoxPlugin {
    fn build(&self, app: &mut bevy::prelude::App) {
        app.add_systems(Startup, spawn_fox);
    }
}

fn spawn_fox(mut commands: Commands, asset_server: Res<AssetServer>) {
    let scene = asset_server.load("Fox.gltf#Scene0");
    commands.spawn((
        Fox::default(),
        SceneBundle {
            scene,
            transform: Transform {
                translation: Vec3::from_array([0., 1., 0.]),
                rotation: Quat::from_rotation_y(std::f32::consts::PI),
                scale: Vec3::splat(1. / 32.),
            },
            ..default()
        },
    ));
}
