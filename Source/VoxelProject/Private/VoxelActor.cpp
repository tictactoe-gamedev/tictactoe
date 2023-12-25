// Fill out your copyright notice in the Description page of Project Settings.


#include "VoxelActor.h"
#include "UObject/ConstructorHelpers.h"
#include "Math/UnrealMathUtility.h"
#include "GameFramework/PlayerController.h"

// Sets default values
AVoxelActor::AVoxelActor()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;
	
	static ConstructorHelpers::FObjectFinder<UStaticMesh> MeshAsset(TEXT("/Script/Engine.StaticMesh'/Engine/BasicShapes/Cube.Cube'"));
    UStaticMeshComponent* MeshComponent = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("VoxelMesh"));
    MeshComponent->SetStaticMesh(MeshAsset.Object);
    RootComponent = MeshComponent;

	VoxelType = 0;
	VoxelAmount = 1;

}

// Called when the game starts or when spawned
void AVoxelActor::BeginPlay()
{
	Super::BeginPlay();
}

// Called every frame
void AVoxelActor::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

void AVoxelActor::SpawnVoxel()
{
    SetRandomValues();
    UE_LOG(LogTemp, Warning, TEXT("VoxelType: %d, VoxelAmount: %d, Coordinates; %d %d %d"), VoxelType, VoxelAmount, RandomX, RandomY, RandomZ);
}

void AVoxelActor::SetRandomValues()
{
    VoxelType = FMath::RandRange(0, 3);
    VoxelAmount = FMath::RandRange(1, 10);

	RandomX = FMath::RandRange(-10, 10);
	RandomY = FMath::RandRange(-10, 10);
	RandomZ = FMath::RandRange(-10, 10);
	SetActorLocation(FVector(RandomX, RandomY, RandomZ));
}