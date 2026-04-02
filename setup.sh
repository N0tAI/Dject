#!/bin/bash
# Dject Project Setup Script for Ubuntu
set -e

echo "Starting project setup for Dject..."

# 1. Install prerequisites
echo "Installing dependencies..."
sudo apt-get update && sudo apt-get install -y \
    wget \
    curl \
    libicu-dev \
    git

# 2. Setup .NET
INSTALL_DIR="$HOME/.dotnet"
mkdir -p "$INSTALL_DIR"

echo "Downloading .NET install script..."
curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh
chmod +x dotnet-install.sh

# Attempt to install .NET 10 (Preview/Future)
echo "Attempting to install .NET 10..."
./dotnet-install.sh --channel 10.0 --install-dir "$INSTALL_DIR" || echo ".NET 10 not available yet."

# Install .NET 8 (Current LTS) as fallback
echo "Installing .NET 8 (LTS)..."
./dotnet-install.sh --channel 8.0 --install-dir "$INSTALL_DIR"

# 3. Configure Environment
echo "Configuring environment variables..."
export DOTNET_ROOT="$INSTALL_DIR"
export PATH="$PATH:$INSTALL_DIR"

# Persist for the session (bash)
if [ -f "$HOME/.bashrc" ]; then
    echo "export DOTNET_ROOT=$INSTALL_DIR" >> "$HOME/.bashrc"
    echo "export PATH=\$PATH:\$DOTNET_ROOT" >> "$HOME/.bashrc"
fi

# 4. Verify installation
echo "Checking dotnet version..."
dotnet --version

# 5. Restore Dependencies
echo "Restoring NuGet packages..."
# If global.json causes issues because .NET 10 is missing, we might need to ignore it for restore
if ! dotnet restore; then
    echo "Restore failed. Retrying without global.json constraints if necessary..."
    dotnet restore --force
fi

echo "Setup complete! You are ready to use Jules with the Dject project."
