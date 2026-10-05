# ✨ Unique Technical Art with 3D VFX

![Unity](https://img.shields.io/badge/Unity-2021.3-000000?style=flat-square&logo=unity&logoColor=white)
![HDRP](https://img.shields.io/badge/HDRP-12.1.7-4A4A4A?style=flat-square&logo=unity&logoColor=white)
![VFX Graph](https://img.shields.io/badge/VFX_Graph-12.1.7-7B3FE4?style=flat-square&logo=unity&logoColor=white)

Unity **VFX Graph**와 **HDRP(High Definition Render Pipeline)**로 GPU 파티클 기반의 3D 테크니컬 아트를 연구한 프로젝트입니다.
코드 대신 노드 그래프로 파티클의 생성·움직임·색상·발광을 설계하고, 3D 모델 데이터를 수십만~백만 개의 파티클로 재구성하는 표현을 실험했습니다.

<!--
## 🎬 결과물

<p align="center">
  <img src="docs/swaying-style.gif" width="49%" alt="Swaying Style">
  <img src="docs/statue-capture.gif" width="49%" alt="Statue Capture Effect">
</p>
-->

---

## 🎨 작품 구성

### 1. Swaying Style — 흔들리는 메시 파티클
> 씬: `Assets/Scene/Swaying Style.unity` · 그래프: `Assets/Swaying Style_study.vfx`

일정 영역(AABox) 안에 메시 파티클을 흩뿌리고, 노이즈와 저항(Drag)으로 부드럽게 흔들리는 움직임을 만든 작품입니다.

| 기능 | 구현 방식 |
|---|---|
| **메시 파티클 출력** | 점·사각형이 아닌 메시 형태로 파티클을 렌더링하고 HDRP Lit 머티리얼로 빛과 그림자를 받도록 구성 |
| **랜덤 회전** | 파티클마다 무작위 회전값을 부여해 반복적인 느낌을 제거 |
| **색상 적용 방법 ①** | 두 가지 색을 지정해 파티클마다 그 사이 색을 할당 |
| **색상 적용 방법 ②** | 이미지에서 컬러 텍스처를 추출해 파티클 색으로 사용 (사이버펑크·일러스트 레퍼런스 이미지 활용) |
| **발광 효과** | HDR 그라디언트 중간 지점에 높은 강도의 색을 넣어 수명 중 한순간 강하게 빛나도록 연출 |
| **외부 제어 파라미터** | `Ratio`, `Interactive Value`를 노출(Exposed)해 인스펙터나 스크립트에서 효과 비율과 반응을 조절 |

### 2. Statue Capture Effect — 조각상을 파티클로 재구성
> 씬: `Assets/Scene/StatueCaptureEffect.unity` · 그래프: `Assets/modeling data.vfx`

3D 조각상 모델을 **포인트 캐시(Point Cache)**로 변환해, 모델 표면을 파티클로 다시 그려낸 작품입니다.

| 기능 | 구현 방식 |
|---|---|
| **포인트 캐시 샘플링** | 모델 표면에서 추출한 **100만 개 포인트**(`Object_3.pcache`)의 위치·노멀·UV를 파티클 초기값으로 사용 |
| **색 표현** | 포인트의 UV로 모델의 베이크 텍스처(`Scene_-_Root_2D_View.png`)를 샘플링해 원본 조각상의 질감을 재현 |
| **방향 기반 움직임** | 포인트 노멀을 파티클의 방향(direction) 값으로 활용해 표면 바깥으로 퍼지는 움직임 구성 |
| **외부 제어 파라미터** | `Modeling Size`로 형태 크기를, `AnimationValue`로 애니메이션 진행 정도를 조절 |

---

## 🛠️ 핵심 기술 포인트

### GPU 기반 대량 파티클
VFX Graph는 시뮬레이션을 GPU 컴퓨트 셰이더에서 처리합니다. CPU 기반 파티클 시스템(Shuriken)으로는 다루기 어려운 **백만 단위 파티클**을 실시간으로 렌더링할 수 있습니다.

### 메시 데이터 → 파티클 데이터 변환
모델의 표면 정보를 포인트 캐시(`.pcache`)로 굽고, 위치·노멀·UV 속성을 각각 파티클의 위치·방향·색상으로 매핑했습니다. 3D 데이터를 다른 표현 형식으로 재해석하는 파이프라인입니다.

### 노출 파라미터로 연출과 로직 분리
그래프 내부 값을 Exposed Property로 꺼내, 그래프를 수정하지 않고도 씬·타임라인·스크립트에서 효과를 조절할 수 있게 설계했습니다.

### HDRP 조명·볼륨 연출
씬마다 별도의 Volume Profile과 HDRI 스카이(`small_cathedral_4k.exr`, `rogland_clear_night_4k.exr`)를 적용해, 파티클의 발광과 반사가 사실적인 조명 환경 안에서 보이도록 구성했습니다.

---

## 📂 프로젝트 구조

```
Assets/
├── Scene/
│   ├── Swaying Style.unity          # ▶ 작품 1
│   └── StatueCaptureEffect.unity    # ▶ 작품 2
├── Swaying Style_study.vfx          # 작품 1 VFX 그래프
├── modeling data.vfx                # 작품 2 VFX 그래프
├── Object_3.pcache                  # 조각상 포인트 캐시 (100만 포인트)
├── Scene_-_Root_2D_View.png         # 조각상 베이크 텍스처
├── Scene_-_Root_Mixed_AO.png        # 조각상 AO 맵
├── dragon.fbx                       # 실험용 3D 모델
├── *.jpg                            # 색상 추출용 레퍼런스 이미지
├── Swaying Style/                   # 작품 1 머티리얼 · 볼륨 프로파일 · HDRI
├── StatueCaptureEffect/             # 작품 2 볼륨 프로파일
└── Settings/                        # HDRP 품질 설정 · 스카이/포그 프로파일
```

---

## ⚙️ 개발 환경

| 항목 | 버전 |
|---|---|
| Unity | **2021.3.9f1** |
| Render Pipeline | HDRP 12.1.7 |
| Visual Effect Graph | 12.1.7 |

## 🚀 실행 방법

1. Unity Hub에서 **Unity 2021.3.9f1**로 이 폴더를 엽니다.
2. `Assets/Scene/` 폴더에서 원하는 씬을 엽니다.
   - `Swaying Style.unity`
   - `StatueCaptureEffect.unity`
3. **Play**를 누르거나, 씬 뷰에서 VFX 오브젝트를 선택하면 효과를 바로 확인할 수 있습니다.
4. VFX 오브젝트의 인스펙터에서 노출 파라미터(`Ratio`, `Interactive Value`, `Modeling Size`, `AnimationValue`)를 바꿔 효과 변화를 실험해 보세요.

> HDRP 프로젝트이므로 그래픽 사양이 낮은 PC에서는 `Assets/Settings/HDRP Performant.asset`으로 품질 설정을 바꾸면 더 원활합니다.
