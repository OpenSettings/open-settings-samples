# 🐳 OpenSettings — Quick Starts: Setting Up The Provider With Docker

![Docker Image Version](https://img.shields.io/docker/v/opensettings/open-settings?logo=docker)

A ready-to-run sample development environment for OpenSettings using Docker Compose. 

📖 Full setup sample available at: [docs.opensettings.net › Quick Starts: Setting Up The Provider With Docker](https://docs.opensettings.net/v1/docs/quick-start-provider-docker.html).

## 🚀 Quick Start

Clone this repo and spin it up:

```bash
git clone https://github.com/OpenSettings/open-settings-samples.git
cd open-settings-samples/versions/v1/quick-starts/4-quick-start-provider-docker
docker-compose up -d
```

> **Note:**  
If you've previously pulled the `opensettings/open-settings:latest` image and .env file uses **IMG_VERSION=latest**, Docker may reuse the cached version.  
To make sure you're using the most up-to-date image, run:

```bash
docker-compose pull
```

🔗 You can also check available image versions at:  
https://hub.docker.com/r/opensettings/open-settings/tags

## 📂 Folder Structure

* **opensettings/** → (Optional) Configuration files, license key, etc.
* .env →  Secrets and environment variables (DB password, etc.)
* docker-compose.yml → Service definitions