use bevy::{
    asset::Handle,
    ecs::{entity::Entity, event::Event},
    pbr::StandardMaterial,
    render::mesh::Mesh,
};

#[derive(Debug, Event)]
pub enum VoxelEvents {
    SpawnVoxel(u64, f32, f32, Handle<Mesh>, Handle<StandardMaterial>),
    DespawnVoxel(Entity),
}
