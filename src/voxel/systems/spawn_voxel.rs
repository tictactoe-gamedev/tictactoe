use bevy::{
    ecs::{event::EventReader, system::Commands},
    math::Vec3,
    pbr::PbrBundle,
    transform::components::Transform,
    utils::default,
};

use crate::voxel::{Voxel, VoxelEvents};

pub fn spawn_voxel(mut commands: Commands, mut voxel_events: EventReader<VoxelEvents>) {
    for voxel_event in voxel_events.read() {
        if let VoxelEvents::SpawnVoxel(voxel_type, voxel_x, voxel_z, voxel_mesh, voxel_material) =
            voxel_event
        {
            commands.spawn((
                PbrBundle {
                    mesh: voxel_mesh.clone(),
                    material: voxel_material.clone(),
                    transform: Transform {
                        translation: Vec3 {
                            x: *voxel_x,
                            y: 0.,
                            z: *voxel_z,
                        },
                        scale: Vec3::splat(0.5),
                        ..default()
                    },
                    ..default()
                },
                Voxel::new(*voxel_type, 1),
            ));
        }
    }
}
