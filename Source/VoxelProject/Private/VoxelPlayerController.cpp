// Fill out your copyright notice in the Description page of Project Settings.


#include "VoxelPlayerController.h"
#include "VoxelActor.h"

const int32 MAX_UNIVERSE_SIZE = 10;

AVoxelPlayerController::AVoxelPlayerController()
{
    // Set this controller to tick every frame
    PrimaryActorTick.bCanEverTick = true;

    MaxVoxelLimit = 20; // Set your desired limit here
    TotalSpawnedVoxels = 0;
}

void AVoxelPlayerController::BeginPlay()
{
    Super::BeginPlay();
}

void AVoxelPlayerController::SetupInputComponent()
{
    Super::SetupInputComponent();

    // Bind the K key press event to the SpawnVoxel function
    InputComponent->BindAction("SpawnVoxel", IE_Pressed, this, &AVoxelPlayerController::SpawnVoxel);
}

FString AVoxelPlayerController::GetVoxelTypeName(int32 NewVoxelType)
{
    FString TypeName;
    switch (NewVoxelType)
    {
        case 0:
            TypeName = TEXT("Gold");
            break;
        case 1:
            TypeName = TEXT("Silver");
            break;
        case 2:
            TypeName = TEXT("Bronze");
            break;
        case 3:
            TypeName = TEXT("Platinum");
            break;
        default:
            TypeName = TEXT("Unknown");
            break;
    }
    return TypeName;
}

void AVoxelPlayerController::UpdateVoxelTypeCount(int32 NewVoxelType)
{

    FString TypeName = GetVoxelTypeName(NewVoxelType);

    // Check if the type is already in the map
    if (VoxelTypeCounts.Contains(NewVoxelType))
    {
        // Increment count if already exists
        VoxelTypeCounts[NewVoxelType]++;
    }
    else
    {
        // Add type to the map if it doesn't exist
        VoxelTypeCounts.Add(NewVoxelType, 1);
    }

    // Log the counts for debugging
    for (const auto& Entry : VoxelTypeCounts)
    {
        UE_LOG(LogTemp, Warning, TEXT("Voxel Type %s Count: %d"), *GetVoxelTypeName(Entry.Key), Entry.Value);
    }
}

void AVoxelPlayerController::SpawnVoxel()
{
    if (TotalSpawnedVoxels >= MaxVoxelLimit)
        {
            UE_LOG(LogTemp, Warning, TEXT("Voxel spawn limit reached. Cannot spawn more voxels."));
            return;
        }

    VoxelType = FMath::RandRange(0, 3);
    VoxelAmount = FMath::RandRange(1, 10);

    RandomX = FMath::RandRange(-MAX_UNIVERSE_SIZE*100, MAX_UNIVERSE_SIZE*100);
    RandomY = FMath::RandRange(-MAX_UNIVERSE_SIZE*100, MAX_UNIVERSE_SIZE*100);
    RandomZ = FMath::RandRange(-MAX_UNIVERSE_SIZE*100, MAX_UNIVERSE_SIZE*100);

    FVector VoxelSpawnLocation = FVector(RandomX, RandomY, RandomZ);
        
    AVoxelActor* NewVoxel = GetWorld()->SpawnActor<AVoxelActor>(
        AVoxelActor::StaticClass(),
        VoxelSpawnLocation,
        FRotator::ZeroRotator
    );

    // Set the properties of the spawned voxel
    if (NewVoxel)
    {
        NewVoxel->VoxelType = VoxelType;
        NewVoxel->VoxelAmount = VoxelAmount;

        UpdateVoxelTypeCount(VoxelType);

        TotalSpawnedVoxels++;
    }

    UE_LOG(LogTemp, Warning, TEXT("VoxelType: %d, VoxelAmount: %d, Coordinates; %d %d %d"), VoxelType, VoxelAmount, VoxelSpawnLocation.X, VoxelSpawnLocation.Y, VoxelSpawnLocation.Z);
}