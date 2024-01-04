mod component;

use bevy::{
    app::{Plugin, Startup, Update},
    asset::AssetServer,
    ecs::system::{Commands, Query, Res, ResMut},
    math::{Quat, Vec3},
    scene::SceneBundle,
    time::Time,
    transform::components::Transform,
    utils::default,
};
use bevy_rand::{prelude::ChaCha8Rng, resource::GlobalEntropy};
use rand::Rng;

use crate::config::WorldConfiguration;

use self::component::Fox;

const FOX_SPEED: f32 = 3.;
const FOX_WAIT_TIME: f64 = 2.;

pub struct FoxPlugin;

impl Plugin for FoxPlugin {
    fn build(&self, app: &mut bevy::prelude::App) {
        app.add_systems(Startup, spawn_fox)
            .add_systems(Update, move_fox);
    }
}

fn spawn_fox(
    mut commands: Commands,
    asset_server: Res<AssetServer>,
    world_config: Res<WorldConfiguration>,
) {
    let scene = asset_server.load("Fox.gltf#Scene0");
    for i in 0..world_config.max_fox_count {
        commands.spawn((
            Fox::default(),
            SceneBundle {
                scene: scene.clone(),
                transform: Transform {
                    translation: Vec3::from_array([0., 1., 0.]),
                    rotation: Quat::from_rotation_y(std::f32::consts::FRAC_PI_4 * (i as f32)),
                    scale: Vec3::splat(1. / 32.),
                },
                ..default()
            },
        ));
    }
}

fn move_fox(
    mut foxes: Query<(&mut Fox, &mut Transform)>,
    mut entropy: ResMut<GlobalEntropy<ChaCha8Rng>>,
    time: Res<Time>,
    world_config: Res<WorldConfiguration>,
) {
    for (mut fox, mut fox_transf) in foxes.iter_mut() {
        let fox_pos_target_diff = fox.target_position - fox_transf.translation;
        if fox_pos_target_diff.length() > FOX_SPEED {
            let movement_direction = fox_pos_target_diff.normalize();
            fox_transf.translation += (movement_direction * FOX_SPEED) * time.delta_seconds();

            let movement_rotation = if movement_direction.x.is_sign_negative() {
                2. * std::f32::consts::PI - movement_direction.z.acos()
            } else {
                movement_direction.z.acos()
            };
            fox_transf.rotation = Quat::from_rotation_y(movement_rotation);
        } else {
            fox.time_from_last_action_taken += time.delta_seconds_f64();
            if fox.time_from_last_action_taken >= FOX_WAIT_TIME && rand::random() {
                let new_target_x = entropy.gen_range(world_config.x_min..world_config.x_max);
                let new_target_z = entropy.gen_range(world_config.z_min..world_config.z_max);
                fox.target_position =
                    Vec3::from_array([new_target_x as f32, 1.0, new_target_z as f32]);
                fox.time_from_last_action_taken = 0.;
            }
        }
    }
}
