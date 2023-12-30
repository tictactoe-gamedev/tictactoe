use bevy::ecs::system::Resource;

#[derive(Resource)]
pub struct VoxelAdjacency {
    pub gold: [f64; 5],
    pub silver: [f64; 5],
    pub copper: [f64; 5],
    pub jade: [f64; 5],
    pub empty: [f64; 5],
}

impl Default for VoxelAdjacency {
    fn default() -> Self {
        VoxelAdjacency {
            gold: [0.4, 0.1, 0.1, 0.3, 0.1],
            silver: [0.1, 0.4, 0.05, 0.4, 0.5],
            copper: [0.05, 0.05, 0.65, 0.2, 0.05],
            jade: [0.1, 0.1, 0.1, 0.6, 0.1],
            empty: [1.; 5],
        }
    }
}
