// Fill out your copyright notice in the Description page of Project Settings.

#include "Kismet/GameplayStatics.h"
#include "EX_MainLevel.h"

#include "EX_GameMode.h"


void AEX_MainLevel::BeginPlay()
{
	Super::BeginPlay();
	UE_LOG(LogTemp,Warning,TEXT("Level Begin Play"));

	AGameModeBase* MyGameMode = UGameplayStatics::GetGameMode(GetWorld());

	checkf(MyGameMode!=nullptr,TEXT("WOW ITS NULL"));

	AEX_GameMode* gameMode = Cast<AEX_GameMode>(MyGameMode);

	checkf(gameMode!=nullptr, TEXT("WE JUST COULDNT CAST IT HEEELP"));

	int a = 64;
	UE_LOG(LogTemp,Warning, TEXT("(%d) Max Sphere count for this level is %d"),a,gameMode->GetMaximumSphereCount());
}
