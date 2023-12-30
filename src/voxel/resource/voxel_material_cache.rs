use bevy::{asset::Handle, ecs::system::Resource, pbr::StandardMaterial};

#[derive(Resource)]
pub struct VoxelMaterialCache {
    pub gold: Handle<StandardMaterial>,
    pub silver: Handle<StandardMaterial>,
    pub copper: Handle<StandardMaterial>,
    pub jade: Handle<StandardMaterial>,
}
