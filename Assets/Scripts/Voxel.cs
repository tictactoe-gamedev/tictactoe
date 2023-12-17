public record Voxel
{
    public int Amount { get; }
    public record GoldVoxel : Voxel
    {
        public GoldVoxel(int amount) : base(amount) { }
    }
    public record SilverVoxel : Voxel
    {
        public SilverVoxel(int amount) : base(amount) { }
    }
    public record BronzeVoxel : Voxel
    {
        public BronzeVoxel(int amount) : base(amount) { }
    }
    public record IronVoxel : Voxel
    {
        public IronVoxel(int amount) : base(amount) { }
    }

    protected Voxel(int amount) => Amount = amount > 0 ? amount : throw new System.ArgumentOutOfRangeException(nameof(amount), "Amount must be non-negative");
}