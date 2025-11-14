# Advanced GPU Instancing Systems

Efficient GPU instancing implementations for general objects and specialized grass rendering.

## 📦 Components

### General Purpose Instancing

**AdvancedGPUInstancer.cs**
- Random rotation/scale controls
- Context menu regeneration
- Area visualization

**SimpleGPUInstancer.cs**
- Grid-based placement
- Minimal setup
- Quick prototyping

### Grass Rendering

**GrassGPUManager.cs**
- ProBuilder mesh-based
- Gradient coloring
- `DrawMeshInstancedIndirect`

**GrassGPUProcedural.cs**
- No mesh required
- Procedural quad generation
- Position/scale noise
- `DrawProcedural` rendering

## 🚀 Quick Setup

### General Instancing
1. Add script to GameObject
2. Assign prefab (MeshFilter + MeshRenderer)
3. Set count and area size
4. Press Play

### Grass Systems
1. Add GrassGPU script to GameObject
2. Assign compute shader and material
3. Configure gradient colors
4. Set area size and density

## 📊 Performance

| System | Max Count | Use Case |
|--------|-----------|----------|
| AdvancedGPU | 100K | Varied props |
| SimpleGPU | 100K | Grid layouts |
| GrassManager | 50K | Custom meshes |
| GrassProcedural | 100K+ | Performance-critical |

## 🔧 Shaders

**GrassColorShader**: Built-in RP, vertex colors
**GrassGPU**: URP, structured buffers
**GrassProcedural_URP**: Procedural vertex generation

**Compute Shaders**: Generate position/scale/color data on GPU

## 📝 Notes

- Enable instancing on materials
- Gradient → 256x1 texture for color variation
- Compatible with Built-in and URP (check shader)
- Use compute shaders for parallel processing
