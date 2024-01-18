mod component;
mod resource;
mod systems;

use bevy::{
    app::{Plugin, Startup, Update},
    asset::Assets,
    ecs::system::{Commands, ResMut},
    pbr::StandardMaterial,
    render::color::Color,
    utils::default,
};

// Components and events can be accessed by other modules
pub use self::{
    component::Voxel,
    systems::{despawn_voxel::DespawnVoxelEvent, spawn_voxel::SpawnVoxelEvent},
};
// Systems can't be accessed by other modules
use self::{
    resource::{voxel_adjacency::VoxelAdjacency, voxel_material_cache::VoxelMaterialCache},
    systems::{despawn_voxel::despawn_voxel, spawn_voxel::spawn_voxel},
};

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
            .init_resource::<VoxelAdjacency>()
            .add_systems(Startup, build_voxel_material_cache)
            .add_systems(Update, spawn_voxel)
            .add_systems(Update, despawn_voxel);
    }
}

fn build_voxel_material_cache(
    mut commands: Commands,
    mut standard_material: ResMut<Assets<StandardMaterial>>,
) {
    let gold = standard_material.add(StandardMaterial {
        base_color: crate::voxel::VOXEL_COLOURS[0],
        metallic: 1.,
        reflectance: 0.75,
        ..default()
    });
    let silver = standard_material.add(StandardMaterial {
        base_color: crate::voxel::VOXEL_COLOURS[1],
        metallic: 0.975,
        reflectance: 0.725,
        ..default()
    });
    let copper = standard_material.add(StandardMaterial {
        base_color: crate::voxel::VOXEL_COLOURS[2],
        metallic: 0.9,
        reflectance: 0.7,
        ..default()
    });
    let jade = standard_material.add(StandardMaterial {
        base_color: crate::voxel::VOXEL_COLOURS[3],
        metallic: 0.25,
        reflectance: 0.25,
        ..default()
    });

    commands.insert_resource(VoxelMaterialCache {
        gold,
        silver,
        copper,
        jade,
    })
}
