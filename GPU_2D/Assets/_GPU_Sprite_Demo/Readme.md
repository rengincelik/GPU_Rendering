
# GPU Sprite Instancing System

Advanced GPU-based sprite instancing with dynamic addition, spherical culling, and frustum culling. Render massive amounts of 2D sprites efficiently.

## ✨ Key Features

- **Dynamic GPU Memory**: Automatic slot allocation using atomic counters
- **Spherical Culling**: Erase sprites within radius around player
- **Frustum Culling**: Only render visible sprites
- **Real-time Debug**: Visual gizmos and performance statistics
- **Procedural Rendering**: `DrawMeshInstancedIndirect` for maximum performance

## 📋 Components

### GPU_Sprite_InstancingController.cs
Main controller managing the instancing pipeline.

**Key Settings:**
- `maxInstances`: Max sprite count (default: 10,000)
- `spawnPerFrame`: Sprites added per frame
- `eraseRadius`: Deletion radius around player
- `enableFrustumCulling`: Camera-based visibility culling

### GPU_Sprite_Instancing.compute
Three GPU kernels:
- **AddNewItems**: Finds free slots, writes new instances
- **SphericalCulling**: Distance-based deletion around player
- **PrepareRenderBuffer**: Filters active instances for rendering

### GPUSpriteUnlit/Standart.shader
Lightweight transparent sprite shader optimized for instancing.

## 🚀 Quick Setup

1. Add `GPU_Sprite_InstancingController` to empty GameObject
2. Assign:
   - Player reference
   - Source sprite (SpriteRenderer)
   - Compute shader
   - Material with GPUSpriteUnlit/Standart shader
3. Configure spawn area and instance count
4. Press Play!

## 📊 Performance

| Instances | Impact | Use Case |
|-----------|--------|----------|
| 1K-5K | Minimal | Small scenes |
| 5K-20K | Low | Large scenes |
| 20K-50K | Moderate | Particle effects |
| 50K+ | Heavy | Extreme scenarios |

**Tips:** Enable frustum culling for large areas, adjust `spawnPerFrame` for target platform.

## 🔧 Technical Overview

**Buffer Pipeline:**
- mainBuffer → newItemsBuffer → renderBuffer
- Atomic counter for thread-safe slot allocation
- AppendBuffer for deletion tracking

**Frame Flow:**
1. Spherical culling (erase)
2. Add new instances
3. Prepare render buffer (frustum cull)
4. Render with indirect draw call

## 📝 Notes

- GPU-driven rendering with minimal CPU overhead
- Compatible with Built-in and URP pipelines
- Thread-safe atomic operations for slot management

