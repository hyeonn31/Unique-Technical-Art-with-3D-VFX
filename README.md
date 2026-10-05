# Unique Technical Art with 3D VFX

![Unity](https://img.shields.io/badge/Unity-2021.3-000000?style=flat-square&logo=unity&logoColor=white)
![HDRP](https://img.shields.io/badge/HDRP-12.1.7-4A4A4A?style=flat-square&logo=unity&logoColor=white)
![VFX Graph](https://img.shields.io/badge/VFX_Graph-12.1.7-7B3FE4?style=flat-square&logo=unity&logoColor=white)

Unity VFX Graph를 공부하면서 만든 작업물 모음입니다.
파티클로 어디까지 표현할 수 있는지 궁금해서 시작했고, 메시 파티클을 흔들어 보는 것부터 3D 조각상을 백만 개의 점으로 다시 그려보는 것까지 해봤습니다. 렌더링은 HDRP를 썼습니다.

<!--
## 결과물

<p align="center">
  <img src="docs/swaying-style.gif" width="49%" alt="Swaying Style">
  <img src="docs/statue-capture.gif" width="49%" alt="Statue Capture Effect">
</p>
-->

## 작업 1. Swaying Style

씬 `Assets/Scene/Swaying Style.unity`
그래프 `Assets/Swaying Style_study.vfx`

정해둔 박스 영역 안에 메시 파티클을 뿌리고, 노이즈와 Drag를 걸어서 천천히 흔들리게 만든 작업입니다.

처음에는 파티클이 다 같은 방향을 보고 있어서 어색했는데, 파티클마다 랜덤 회전값을 주니까 훨씬 자연스러워졌습니다. 그래프 안에도 이 부분을 따로 묶어서 메모해 뒀습니다.

색을 입히는 방법은 두 가지로 실험했습니다.

- 두 가지 색을 정해두고 파티클마다 그 사이 색을 랜덤으로 주는 방법
- 이미지에서 색을 뽑아와서 파티클에 입히는 방법 (`Assets` 폴더의 사이버펑크, 일러스트 이미지들이 이 용도입니다)

발광은 수명 동안 색이 바뀌는 그라디언트 중간에 HDR 값을 아주 높게 넣어서, 파티클이 살아있는 동안 한 순간 번쩍 빛나도록 했습니다.

`Ratio`와 `Interactive Value`는 밖으로 빼 둬서 인스펙터에서 바로 바꿔볼 수 있습니다.

## 작업 2. Statue Capture Effect

씬 `Assets/Scene/StatueCaptureEffect.unity`
그래프 `Assets/modeling data.vfx`

조각상 모델을 포인트 캐시로 구워서 파티클로 다시 세운 작업입니다. `Object_3.pcache`에 모델 표면에서 뽑은 점이 100만 개 들어 있고, 각 점의 위치, 노멀, UV를 그대로 파티클에 넘겼습니다.

색은 UV로 조각상의 베이크 텍스처(`Scene_-_Root_2D_View.png`)를 읽어서 원래 모델의 질감이 보이도록 했고, 노멀은 파티클의 방향 값으로 써서 표면 바깥쪽으로 움직이게 했습니다.

`Modeling Size`로 전체 크기를, `AnimationValue`로 움직임의 정도를 조절할 수 있습니다.

## 하면서 알게 된 것

VFX Graph는 GPU에서 돌아가기 때문에 파티클 수가 백만 단위가 되어도 실시간으로 버틴다는 게 가장 인상적이었습니다. 기존 파티클 시스템(Shuriken)으로는 엄두가 안 나는 양입니다.

포인트 캐시를 다루면서는 결국 모델도 위치, 방향, 색이라는 데이터 묶음이라는 걸 체감했습니다. 그 데이터를 어떤 속성에 연결하느냐에 따라 같은 모델이 완전히 다른 모습이 됩니다.

또 값을 Exposed Property로 빼 두면 그래프를 열지 않고도 씬이나 타임라인에서 조절할 수 있어서, 연출을 바꿔볼 때 훨씬 편했습니다.

씬마다 Volume Profile과 HDRI(`small_cathedral_4k.exr`, `rogland_clear_night_4k.exr`)를 따로 잡아서, 발광이나 반사가 조명 안에서 어떻게 보이는지도 같이 확인했습니다.

## 폴더 구조

```
Assets/
├── Scene/
│   ├── Swaying Style.unity          # 작업 1
│   └── StatueCaptureEffect.unity    # 작업 2
├── Swaying Style_study.vfx          # 작업 1 그래프
├── modeling data.vfx                # 작업 2 그래프
├── Object_3.pcache                  # 조각상 포인트 캐시 (100만 개)
├── Scene_-_Root_2D_View.png         # 조각상 텍스처
├── Scene_-_Root_Mixed_AO.png        # 조각상 AO 맵
├── dragon.fbx                       # 테스트용 모델
├── *.jpg                            # 색 추출용 이미지
├── Swaying Style/                   # 작업 1 머티리얼, 볼륨, HDRI
├── StatueCaptureEffect/             # 작업 2 볼륨
└── Settings/                        # HDRP 품질 설정, 스카이/포그
```

## 개발 환경

- Unity 2021.3.9f1
- HDRP 12.1.7
- Visual Effect Graph 12.1.7

## 실행 방법

1. Unity Hub에서 2021.3.9f1 버전으로 프로젝트를 엽니다.
2. `Assets/Scene/`에서 보고 싶은 씬을 엽니다.
3. Play를 누르거나 씬 뷰에서 VFX 오브젝트를 선택하면 바로 재생됩니다.
4. 인스펙터에서 `Ratio`, `Interactive Value`, `Modeling Size`, `AnimationValue` 값을 바꿔보면 효과가 어떻게 달라지는지 볼 수 있습니다.

사양이 낮은 PC에서 버벅인다면 품질 설정을 `Assets/Settings/HDRP Performant.asset`으로 바꿔 보세요.
