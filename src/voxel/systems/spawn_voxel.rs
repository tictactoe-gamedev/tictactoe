use bevy::{
    asset::{AssetServer, Assets},
    ecs::{
        event::{Event, EventReader},
        system::{Commands, Query, Res, ResMut, SystemParam},
    },
    math::Vec3,
    pbr::{PbrBundle, StandardMaterial},
    transform::components::Transform,
    utils::default,
};
use bevy_rand::{prelude::ChaCha8Rng, resource::GlobalEntropy};
use rand_core::RngCore;

use crate::{config::WorldConfiguration, voxel::Voxel};

#[derive(Event)]
pub struct SpawnVoxelEvent;

#[derive(SystemParam)]
pub struct SpawnVoxelSystemParam<'w, 's> {
    world_config: Res<'w, WorldConfiguration>,
    asset_server: Res<'w, AssetServer>,
    standard_materials: ResMut<'w, Assets<StandardMaterial>>,
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
        let col_count = system_param.world_config.z_max - system_param.world_config.z_min;
        let max_height = 3;

        let mut voxel_count = if let Ok(count) = i32::try_from(system_param.voxels.iter().count()) {
            count
        } else {
            bevy::log::error!("Failed to get voxel count.");
            return;
        };

        for _event in event_reader.read() {
            let voxel_type = system_param.entropy.next_u32() % 4;
            let x = (voxel_count % row_count) + system_param.world_config.x_min;
            let y = voxel_count / (row_count * col_count);
            let z = ((voxel_count / row_count) % col_count) + system_param.world_config.z_min;
            bevy::log::warn!("{voxel_count} {x} {y} {z}");

            if y >= max_height {
                break;
            }

            let mesh_handle = system_param.asset_server.load("cube.gltf#Mesh0/Primitive0");

            let material_handle = system_param.standard_materials.add(StandardMaterial {
                base_color: crate::voxel::VOXEL_COLOURS[voxel_type as usize],
                ..default()
            });

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
