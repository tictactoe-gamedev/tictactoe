use bevy::{
    asset::AssetServer,
    ecs::{
        event::{Event, EventReader},
        system::{Commands, Local, Query, Res, ResMut, SystemParam},
    },
    math::Vec3,
    pbr::PbrBundle,
    transform::components::Transform,
    utils::default,
};
use bevy_rand::{prelude::ChaCha8Rng, resource::GlobalEntropy};
use rand::distributions::Distribution;

use crate::{
    config::WorldConfiguration,
    voxel::{resource::voxel_adjacency::VoxelAdjacency, Voxel, VoxelMaterialCache},
};

#[derive(Event)]
pub struct SpawnVoxelEvent;

#[derive(SystemParam)]
pub struct SpawnVoxelSystemParam<'w, 's> {
    world_config: Res<'w, WorldConfiguration>,
    voxel_adjacency: Res<'w, VoxelAdjacency>,
    asset_server: Res<'w, AssetServer>,
    voxel_materials: Res<'w, VoxelMaterialCache>,
    entropy: ResMut<'w, GlobalEntropy<ChaCha8Rng>>,
    voxels: Query<'w, 's, (&'static Transform, &'static Voxel)>,
}

pub fn spawn_voxel(
    mut commands: Commands,
    mut system_param: SpawnVoxelSystemParam,
    mut voxel_count: Local<i32>,
    mut event_reader: EventReader<SpawnVoxelEvent>,
) {
    if !event_reader.is_empty() {
        let row_count = system_param.world_config.x_max - system_param.world_config.x_min;
        let max_height = system_param.world_config.y_max - system_param.world_config.y_min;
        let col_count = system_param.world_config.z_max - system_param.world_config.z_min;

        for _event in event_reader.read() {
            let x = (*voxel_count % row_count) + system_param.world_config.x_min;
            let y = *voxel_count / (row_count * col_count);
            let z = ((*voxel_count / row_count) % col_count) + system_param.world_config.z_min;

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
                        0 => system_param.voxel_adjacency.gold,
                        1 => system_param.voxel_adjacency.silver,
                        2 => system_param.voxel_adjacency.copper,
                        4 => system_param.voxel_adjacency.jade,
                        _ => system_param.voxel_adjacency.empty,
                    })
                    .fold(system_param.voxel_adjacency.empty, |accum, cur| {
                        [
                            accum[0] * cur[0],
                            accum[1] * cur[1],
                            accum[2] * cur[2],
                            accum[3] * cur[3],
                            accum[4] * cur[4],
                        ]
                    });
                match rand::distributions::WeightedIndex::new(weights) {
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

            if voxel_type < 4 {
                let mesh_handle = system_param.asset_server.load("cube.gltf#Mesh0/Primitive0");

                let material_handle = match voxel_type {
                    0 => system_param.voxel_materials.gold.clone(),
                    1 => system_param.voxel_materials.silver.clone(),
                    2 => system_param.voxel_materials.copper.clone(),
                    3 => system_param.voxel_materials.jade.clone(),
                    _ => {
                        bevy::log::warn!("Empty voxel does not have material.");
                        return;
                    }
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
                            scale: Vec3::splat(0.45),
                            ..default()
                        },
                        ..default()
                    },
                    Voxel::new((x, y, z), voxel_type, 1),
                ));
            }
            *voxel_count += 1;
        }
    }
}
