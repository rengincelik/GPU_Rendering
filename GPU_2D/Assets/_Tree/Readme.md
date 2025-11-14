# Procedural Tree System

ScriptableObject-based system for creating procedural branching structures with circular nodes. Uses Gizmos for visual debugging and recursive branch generation.

## ✨ Key Features

- **Recursive Branching**: Create complex tree structures with parent-child relationships
- **Circular Nodes**: Segmented circles at branch endpoints
- **Random Generation**: Parameterized random values for organic variation
- **Visual Debugging**: Real-time Gizmo visualization in Scene view
- **Two End Types**: Dot or Branch continuation at node endpoints

## 📋 Components

### BranchData.cs (ScriptableObject)
Base branch configuration with fixed values.

**Properties:**
- `radius`: Circle radius (1-10)
- `segments`: Number of circle points (3-10)
- `percent`: Circle completion percentage (10-90%)
- `branchLength`: Length of the branch (1-10)
- `endType`: Dot or Branch continuation
- `childBranch`: Reference to next branch level

### RandomBranchData.cs (ScriptableObject)
Branch configuration with randomizable ranges.

**Features:**
- Range-based parameters (min/max)
- `Randomize()` method for runtime value generation
- Cached runtime values for consistency
- Custom editor with randomize button

### MultiCircleGizmo.cs
Main visualization component - renders recursive tree structure.

**Functionality:**
- Draws branches from center point
- Renders segmented circles at endpoints
- Recursively processes child branches
- Respects rotation angles for natural growth

### CircleGizmo.cs
Simple single-circle visualizer for testing.

**Features:**
- Adjustable segment count and radius
- Percentage-based circle completion
- Center lines and dots at segments
- Extra line indicator for empty arc

### BranchRuntime.cs
Runtime data structure for procedurally generated trees.

**Static Method:**
- `CreateRandom(depth, maxDepth)`: Generates random tree structure
- Automatic depth limiting
- Probabilistic branch/dot selection

## 🚀 Quick Setup

### Manual Tree Creation
1. Create BranchData assets (Right-click → Create → Tree/BranchData)
2. Configure radius, segments, length, and percent
3. Assign child branches for recursive structure
4. Add `MultiCircleGizmo` to GameObject
5. Assign root branch and view in Scene

### Random Tree Generation
1. Create RandomBranchData (Right-click → Create → Tree/RandomBranchData)
2. Set min/max ranges for parameters
3. Click "Randomize Values" in Inspector
4. Preview runtime values in editor

## 🎨 Visual Elements

**Colors:**
- Brown (0.55, 0.27, 0.07): Branch lines
- Green: Dot endpoints
- Red: Circle segment points
- Blue: Extra line indicators

## 🔧 Technical Details

### Recursive Structure
```
Root Branch
├─ Circle Segments (percent%)
│  ├─ Child Branch 1 (if endType = Branch)
│  │  └─ ... (recursive)
│  └─ Dot (if endType = Dot)
```

### Angle Calculation
- Segments evenly distributed across percentage arc
- Rotation angle propagates through tree hierarchy
- Default upward direction (90°)

## 📝 Use Cases

- Procedural tree/plant generation
- Branch-based level design
- Organic pattern visualization
- Debug visualization for hierarchical data
- Prototype tool for growth algorithms

## 💡 Tips

- Keep `maxDepth` ≤ 5 for performance
- Use lower segment counts (3-6) for stylized trees
- Higher percent values create fuller circles
- Combine fixed and random branches for controlled variation
