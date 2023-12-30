mod component;
mod systems;

use bevy::{
    app::{Plugin, Update},
    render::color::Color,
};

// Components and events can be accessed by other modules
pub use self::{
    component::Voxel,
    systems::{despawn_voxel::DespawnVoxelEvent, spawn_voxel::SpawnVoxelEvent},
};
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
        app.add_event::<SpawnVoxelEvent>()
            .add_event::<DespawnVoxelEvent>()
            .add_systems(Update, spawn_voxel)
            .add_systems(Update, despawn_voxel);
    }
}
