# BinLali-2D — Contexto del Proyecto

## Resumen

Juego 2D de plataformas/acción hecho en **Unity 6000.3.11f1** con **URP 2D**. Es un proyecto final ("Proyecto Final 2D"). La jugadora recorre un nivel, dispara tokens (Z = horizontal, X = vertical), mata enemigos (diablos) que dropean tokens. Al juntar 20 tokens aparece el jefe final. Matar al jefe = victoria, morir = derrota.

**4 personajes jugables:** Natalia, Susy, Nani, Sopas.

---

## Stack técnico

| Componente | Versión |
|---|---|
| Unity | 6000.3.11f1 |
| Render Pipeline | URP 17.3.0 |
| Template | universal-2d@6.1.2 |
| Input | Input System 1.19.0 + legacy Input Manager |
| UI | TextMeshPro, uGUI 2.0.0 |
| 2D | SpriteShape 13.0.0, Tilemap Extras 6.0.1, 2D Animation 13.0.4 |

---

## Estructura del proyecto

```
BinLali-2D/
├── Assets/
│   ├── Scenes/
│   │   ├── NivelObsolescencia.unity   ← Escena principal del juego
│   │   └── SampleScene.unity          ← Vacía (no se usa)
│   ├── Scripts/                        ← 10 scripts C#
│   │   ├── GameManager.cs             ← Flujo del juego, estados, selección
│   │   ├── SeleccionPersonaje.cs      ← Datos de sprites por personaje
│   │   ├── PersonajeAnimationController.cs ← LateUpdate, override de sprite
│   │   ├── JugadoraMovimiento.cs      ← Movimiento y salto
│   │   ├── VidaJugadora.cs            ← Vida e invencibilidad
│   │   ├── Disparo.cs                 ← Disparo de tokens (Z/X)
│   │   ├── SpawnerEnemigos.cs         ← Spawn de diablos
│   │   ├── EnemigoDiablo.cs           ← IA del enemigo (persigue)
│   │   ├── JefeFinal.cs               ← Jefe: vida, movimiento, ataques
│   │   ├── ProyectilJefe.cs           ← Daño del proyectil del jefe
│   │   └── CameraFollow.cs            ← Cámara sigue a la jugadora
│   ├── Sprites/
│   │   ├── SpritesNiña.png  (46 sprites) ← Susy — animaciones base
│   │   ├── SpritesNani.png  (46 sprites) ← Nani
│   │   ├── SpritesNat.png   (39 sprites) ← Natalia
│   │   ├── SpritesSopas.png (48 sprites) ← Sopas
│   │   ├── SpritesDiablo.png (9 sprites) ← Enemigo
│   │   └── ... (fondos, UI, suelo, etc.)
│   ├── Animations/
│   │   ├── Jugadora.controller  ← Controlador de animación de la jugadora
│   │   ├── Idle.anim            ← 6 frames (usa SpritesNiña)
│   │   ├── Walk.anim            ← 6 frames (usa SpritesNiña)
│   │   ├── Disparo.anim         ← 1 frame  (usa SpritesNiña)
│   │   ├── Diablo.controller    ← Controlador del enemigo
│   │   └── DiabloIdle.anim      ← 3 frames
│   ├── Prefabs/
│   │   ├── Diablo.prefab
│   │   ├── ProyectilJefe.prefab
│   │   └── Token.prefab
│   └── InputSystem_Actions.inputactions
```

---

## Scripts clave — detalle

### GameManager.cs
Máquina de estados del juego:
- `0` = PantallaInicio → Enter → `1`
- `1` = PantallaHistoria → Enter → `2`
- `2` = PantallaSeleccion → Teclas 1-4 eligen personaje → `3`
- `3` = Gameplay

Referencias públicas: pantallas (inicio, historia, seleccion, victoria, derrota), jefe, textoTokens, seleccionPersonaje.

`Time.timeScale = 0f` al inicio (pantallas), `= 1f` al empezar gameplay.

### SeleccionPersonaje.cs (reescrito)
Contiene clase anidada `PersonajeData` con:
- `Sprite[] spritesIdle` (6 frames)
- `Sprite[] spritesWalk` (6 frames)
- `Sprite spriteDisparo` (1 frame)

4 instancias públicas: `natalia`, `susy`, `nani`, `sopas`.

Propiedades estáticas:
- `PersonajeActual` (int, 0=nada, 1=Natalia, 2=Susy, 3=Nani, 4=Sopas)
- `Instancia` (singleton)

Método `GetPersonajeData(int numero)` devuelve los datos del personaje.

### PersonajeAnimationController.cs (nuevo)
- `[DefaultExecutionOrder(100)]` — se ejecuta después del Animator
- `[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]`
- En `LateUpdate()`: lee el estado actual del Animator (Idle/Walk/Disparo), calcula el índice de frame usando `normalizedTime`, y setea `spriteRenderer.sprite` al sprite correcto del personaje seleccionado.

### JugadoraMovimiento.cs
Movimiento horizontal con `Input.GetAxis("Horizontal")`, salto con `Jump`. Voltea el sprite con `localScale`. El Animator recibe `velocidadX` (float) para transiciones Idle↔Walk. En `Start()` agrega automáticamente `PersonajeAnimationController` si no existe.

### Disparo.cs
Tecla Z dispara horizontal (`Vector2.right * dirección`), tecla X dispara hacia arriba (`Vector2.up`). Activa `anim.SetBool("disparando", true)` por 0.3s.

---

## Estructura del Animator (Jugadora.controller)

**Parámetros:** `velocidadX` (Float), `disparando` (Bool)

**Estados:**
- **Idle** (default) → 6 frames, sprites Niña 0-5
- **Walk** → 6 frames, sprites Niña 6-11
- **Disparo** → 1 frame, sprite Niña 28

**Transiciones:**
| De | A | Condición |
|----|---|-----------|
| Idle → Walk | `velocidadX < 0.1` |
| Walk → Idle | `velocidadX > 0.1` |
| Idle → Disparo | `disparando == true` |
| Disparo → Idle | `disparando == false` |

**Problema original:** Los 3 animation clips (Idle, Walk, Disparo) referencian sprites de `SpritesNiña.png` (Susy). El Animator escribe `m_Sprite` en el SpriteRenderer cada frame, sobrescribiendo cualquier cambio manual. La solución implementada usa `LateUpdate` para re-sobrescribir con los sprites del personaje correcto.

---

## Layout de sprites por personaje

### Susy (SpritesNiña.png — 46 sprites, 6 columnas)
| Fila | Y | Índices | Uso |
|------|---|---------|-----|
| 1 | ~512 | 0-5 (47×87) | **Idle** |
| 2 | ~437 | 6-11 (56×73) | **Walk** |
| 3 | ~364 | 12-18 (57×71) | Sin uso |
| 4 | ~290 | 19-20 (42×73) | Sin uso |
| 5 | ~216 | 21-27 (varios) | Sin uso |
| 6 | ~70 | 28 | **Disparo** |
| Varias | ~0-143 | 29-45 | Sin uso |

### Nani (SpritesNani.png — 46 sprites)
Mismo layout general: índices 0-5 Idle (~44×83), índices 6-11 Walk (~56×75).

### Natalia (SpritesNat.png — 39 sprites)
Layout diferente. Índices 0-5 = Idle (~43×80). Walk y Disparo en posiciones distintas — revisar visualmente en el Sprite Editor.

### Sopas (SpritesSopas.png — 48 sprites)
Layout 6×8 columnas. Índices 0-5 Idle (~49×89), índices 6-11 Walk (~48×80).

---

## Estado actual del trabajo

### Hecho
- [x] `SeleccionPersonaje.cs` reescrito con arrays de sprites por personaje + clase `PersonajeData`
- [x] `PersonajeAnimationController.cs` creado — LateUpdate override del sprite según Animator state + personaje seleccionado
- [x] `GameManager.cs` — auto-find de `SeleccionPersonaje` en Awake si es null
- [x] `JugadoraMovimiento.cs` — agrega `PersonajeAnimationController` automáticamente en Start

### Pendiente (requiere Unity Editor)
- [ ] **En la escena `NivelObsolescencia.unity`, seleccionar el GameObject "SeleccionPersonaje" y arrastrar los sprites a los arrays en el Inspector:**
  - `Natalia > Sprites Idle` (6), `Sprites Walk` (6), `Sprite Disparo` (1)
  - `Susy > Sprites Idle` (SpritesNiña_0 a _5), `Sprites Walk` (SpritesNiña_6 a _11), `Sprite Disparo` (SpritesNiña_28)
  - `Nani > Sprites Idle` (SpritesNani_0 a _5), `Sprites Walk` (SpritesNani_6 a _11), `Sprite Disparo`
  - `Sopas > Sprites Idle` (SpritesSopas_0 a _5), `Sprites Walk` (SpritesSopas_6 a _11), `Sprite Disparo`
- [ ] Guardar la escena
- [ ] Probar que la selección de personajes funciona con los 4 personajes
- [ ] Para Natalia: revisar visualmente el spritesheet para identificar cuáles son walk y disparo (tiene solo 39 sprites, layout distinto)

### Posibles mejoras futuras
- Crear animaciones de disparo multi-frame (actualmente es 1 solo frame)
- Agregar animación de salto
- Agregar animación de daño/muerte para la jugadora
- Crear `AnimatorOverrideController` por personaje como alternativa más limpia (evita el LateUpdate hack)
- Separar sprites en `Resources/` para carga dinámica
