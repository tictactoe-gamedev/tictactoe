use bevy::ecs::{
    entity::Entity,
    event::{Event, EventReader},
    system::Commands,
};

#[derive(Event)]
pub struct DespawnVoxelEvent(pub Entity);

pub fn despawn_voxel(mut commands: Commands, mut voxel_events: EventReader<DespawnVoxelEvent>) {
    for voxel_event in voxel_events.read() {
        if let Some(mut entity) = commands.get_entity(voxel_event.0) {
            entity.despawn();
        };
    }
}
