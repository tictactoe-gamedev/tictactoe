use bevy::ecs::{event::EventReader, system::Commands};

use crate::voxel::VoxelEvents;

pub fn despawn_voxel(mut commands: Commands, mut voxel_events: EventReader<VoxelEvents>) {
    for voxel_event in voxel_events.read() {
        if let VoxelEvents::DespawnVoxel(ent) = voxel_event {
            if let Some(mut entity) = commands.get_entity(*ent) {
                entity.despawn();
            };
        }
    }
}
