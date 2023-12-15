// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/GameModeBase.h"
#include "EX_GameMode.generated.h"

/**
 * 
 */
UCLASS()
class UNREAL_PROJECT_API AEX_GameMode : public AGameModeBase
{
	GENERATED_BODY()

public:
	int GetMaximumSphereCount();
	
protected:
	UPROPERTY(EditAnywhere)
	int MaximumSphereCount;

	virtual void BeginPlay() override;
};
