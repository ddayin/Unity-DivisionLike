# DivisionLike Unity Project Summary
develop a game like Tom Clancy's The Division by using Unity

![image](https://user-images.githubusercontent.com/29808782/179521804-14d527cf-0543-4ddd-bb8e-feed2149be53.png)



https://github.com/user-attachments/assets/cce652bf-2088-4db2-9cc6-4a7fbf413998




# Project Purpose
1. I like TPS(Third Person Shooter) games! So I want to make my own game like division
2. personal toy project
3. to learn Unity & C#

# Key Features
- control player character
- player character animation
- control weapons
- spawn enemies
- HUD to display information
- support HDRP

# Third Party Plugins
- Outline Effect
- MiniMap
- EZ Camera Shake
- LenaPool
- iTween

# TO DO
- enemy AI
- add weapons
- improve mini map
- improve snow particle
- improve background objects like building
- add inventory system
- add various weapons

# PC Specification
- OS : Windows 11 64bit
- CPU : AMD Ryzen 9 5900X 12-Core
- RAM : 32GB
- SSD : Samsung SSD 870 QVO 1TB
- GPU : NVIDIA GeForce RTX 3060

# Development Tools (02/08/2024)
- Engine : Unity 2023.2.61f1
- Language : C#
- IDE : Jetbrains Rider 2023.3.3

# Player Character
- Player.cs : 플레이어의 component들을 가지고 있다.
- PlayerAnimation.cs : 플레이어의 애니메이션
- PlayerFootstepSound.cs : 플레이어의 발자국 소리
  - 바닥 텍스처 타입에 따라 다른 발소리 재생
- PlayerHandleGrenade.cs : 수류탄 투척
- PlayerHealth.cs : 플레이어의 HP
- PlayerInput.cs : 플레이어 입력 처리
- PlayerInventory.cs : 플레이어의 인벤토리
- PlayerMovement.cs : 플레이어 이동
- PlayerStats.cs : 플레이어 stat

# Weapons
- Grenade.cs : 슈류탄
- GrenadeHandler.cs : 슈류탄 처리
- Weapon.cs : 무기
- WeaponHandler.cs : 무기 처리

# Enemy
- EnemyAttack.cs : 적 캐릭터의 공격
- EnemyDropItem.cs : 적 캐릭터가 죽으면 아이템을 떨어뜨린다.
- EnemyHealth.cs : 적 캐릭터의 HP
- EnemyInventory.cs : 적 캐릭터의 인벤토리
- EnemyMovement.cs : 적 캐릭터의 이동
- EnemyStats.cs : 적 캐릭터의 스탯

# GUI
- AmmoBoxIcon.cs : 탄약 상자 아이콘
- CircularHit.cs : 플레이어를 기준으로 상대에게 맞은 방향으로 이미지 회전해서 표시
- CommonPopup.cs
- CrosshairAR.cs : AR 총의 crosshair
- CrosshairController.cs : crosshair 처리
- CrosshairHandler.cs
- CrosshairMakarov.cs : Makarov 총의 crosshair
- EnemyUI.cs : 적 캐릭터의 UI
- FloatingText.cs : 텍스트
- FloatingTextController.cs : 텍스트 처리
- MinimapHit.cs : 미니맵에 어느 방향으로 플레이어 캐릭터가 맞았는지 표시
- PlayerHUD.cs : 플레이어의 HUD
- screenHUD.cs : 미니맵, 현재 경험치, 현재 레벨, 현재 장전된 총알 등을 포함한 UI 표시
- ToastPopup.cs

# Effect
- GrenadeCircleEffect.cs : 폭파 범위 이펙트
- ItemDropEffect.cs : 아이템을 떨어뜨린 지점에 라인을 그린다.
- PlayerOutlineEffect.cs : 플레이어에 아웃라인을 그리는 이펙트

# Managers
- EffectManager.cs : 이펙트 관리자
- EnemyManager.cs : 적 관리자
- GameOverManager.cs : 게임 오버 관리자
- PauseManager.cs : 일시 정지 관리자
- PlayScene.cs : 플레이 씬
- PopupManager.cs : 팝업 관리자
- SceneController.cs : 씬을 불러들이고 관리한다.
- ScoreManager.cs : 점수 관리자
- SoundController.cs : 사운드 처리

# etc
- CameraControl.cs
- HeadBobber.cs
- RandomAnimationPoint.cs
- RandomParticlePoint.cs
- VolumeHandler.cs
- OilBarrel.cs
- Singleton.cs
