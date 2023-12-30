use bevy::{
    asset::AssetServer,
    ecs::{
        event::{Event, EventReader},
        system::{Commands, Query, Res, ResMut, SystemParam},
    },
    math::{Vec3, Vec4},
    pbr::PbrBundle,
    transform::components::Transform,
    utils::default,
};
use bevy_rand::{prelude::ChaCha8Rng, resource::GlobalEntropy};
use rand::distributions::Distribution;

use crate::{
    config::WorldConfiguration,
    voxel::{Voxel, VoxelMaterialCache},
};

#[derive(Event)]
pub struct SpawnVoxelEvent;

#[derive(SystemParam)]
pub struct SpawnVoxelSystemParam<'w, 's> {
    world_config: Res<'w, WorldConfiguration>,
    asset_server: Res<'w, AssetServer>,
    voxel_materials: Res<'w, VoxelMaterialCache>,
    entropy: ResMut<'w, GlobalEntropy<ChaCha8Rng>>,
    voxels: Query<'w, 's, (&'static Transform, &'static Voxel)>,
}

pub fn spawn_voxel(
    mut commands: Commands,
    mut system_param: SpawnVoxelSystemParam,
    mut event_reader: EventReader<SpawnVoxelEvent>,
) {
    if !event_reader.is_empty() {
        let row_count = system_param.world_config.x_max - system_param.world_config.x_min;
        let max_height = system_param.world_config.y_max - system_param.world_config.y_min;
        let col_count = system_param.world_config.z_max - system_param.world_config.z_min;

        let mut voxel_count = if let Ok(count) = i32::try_from(system_param.voxels.iter().count()) {
            count
        } else {
            bevy::log::error!("Failed to get voxel count.");
            return;
        };

        for _event in event_reader.read() {
            let x = (voxel_count % row_count) + system_param.world_config.x_min;
            let y = voxel_count / (row_count * col_count);
            let z = ((voxel_count / row_count) % col_count) + system_param.world_config.z_min;

            if y >= max_height {
                break;
            }

            let voxel_type = {
                let adjacent = [
                    (x - 1, y, z),
                    (x + 1, y, z),
                    (x, y - 1, z),
                    (x, y + 1, z),
                    (x, y, z - 1),
                    (x, y, z + 1),
                ];
                let weights = system_param
                    .voxels
                    .iter()
                    .filter(|(_, vox)| adjacent.contains(vox.position()))
                    .map(|(_, vox)| match vox.voxel_type() {
                        0 => Vec4::from_array([0.4, 0.1, 0.1, 0.4]),
                        1 => Vec4::from_array([0.05, 0.5, 0.05, 0.4]),
                        2 => Vec4::from_array([0.05, 0.05, 0.7, 0.2]),
                        _ => Vec4::from_array([0.1, 0.1, 0.1, 0.7]),
                    })
                    .fold(Vec4::splat(1.), |accum, cur| accum * cur);
                match rand::distributions::WeightedIndex::new(weights.to_array()) {
                    Ok(distribution) => {
                        if let Ok(idx) =
                            u32::try_from(distribution.sample(&mut *system_param.entropy))
                        {
                            idx
                        } else {
                            bevy::log::error!("Failed to cast usize index to u32.");
                            return;
                        }
                    }
                    Err(err) => {
                        bevy::log::error!("Failed to create weighted index distribution. '{err}'");
                        return;
                    }
                }
            };

            let mesh_handle = system_param.asset_server.load("cube.gltf#Mesh0/Primitive0");

            let material_handle = match voxel_type {
                0 => system_param.voxel_materials.gold.clone(),
                1 => system_param.voxel_materials.silver.clone(),
                2 => system_param.voxel_materials.copper.clone(),
                _ => system_param.voxel_materials.jade.clone(),
            };

            commands.spawn((
                PbrBundle {
                    mesh: mesh_handle,
                    material: material_handle,
                    transform: Transform {
                        translation: Vec3 {
                            x: x as f32,
                            y: y as f32,
                            z: z as f32,
                        },
                        scale: Vec3::splat(0.5),
                        ..default()
                    },
                    ..default()
                },
                Voxel::new((x, y, z), voxel_type, 1),
            ));
            voxel_count += 1;
        }
    }
}
