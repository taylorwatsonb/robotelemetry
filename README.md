
# 3D Robot Telemetry Visualization

A real-time 3D visualization system for robot telemetry data using Three.js and SignalR. This project demonstrates remote monitoring and control capabilities for robotic systems.

## Features

- Real-time 3D visualization of robot position and orientation
- Manual control interface for robot movement
- Live telemetry data display
- Motion trail visualization
- Interactive 3D camera controls (orbit, zoom, pan)
- Real-time WebSocket communication using SignalR

## Technologies

- Backend: ASP.NET Core 7.0
- Frontend: Three.js
- Real-time Communication: SignalR
- 3D Graphics: WebGL

## Use Cases

- Remote robot monitoring
- Testing robot control algorithms
- Training simulation
- Development and debugging of robotic systems
- Integration testing for automation systems

## Controls

- **Manual Movement**:
  - Forward/Backward
  - Up/Down
  - Left/Right
- **Camera**:
  - Left click + drag: Rotate view
  - Right click + drag: Pan
  - Scroll: Zoom

## Visual Features

- Detailed 3D robot model with animated components
- Grid reference plane
- Coordinate axes
- Motion trail
- Real-time telemetry display panel
- Dynamic lighting and shadows

## Getting Started

1. Clone this Repl
2. Click Run
3. The application will start on port 3000
4. Use the control panel to interact with the robot

## Architecture

- **Server**: Handles state management and broadcasts updates
- **Client**: Renders 3D visualization and processes user input
- **Communication**: Bi-directional real-time updates via SignalR
