
provider "azurerm" {
  version = "=2.1.0"
  features {}
}

terraform {
    backend "azurerm" {
        # filled in by azure devops runner
    }
}

// collect parameters
variable "appservice_name" {
    type    = string
}

variable "build_version" {
  type      = string
}

// specify extras
resource "random_string" "random" {
  length    = 4
  upper     = false
  special   = false
}

// configure data sources
resource "azurerm_resource_group" "rg" {
    name        = "rg-${var.appservice_name}-${random_string.random.result}"
    location    = "East US"
}

// specify resources to create
resource "azurerm_app_service_plan" "plan" {
    name                    = "plan-${var.appservice_name}-${random_string.random.result}"
    resource_group_name     = "${azurerm_resource_group.rg.name}"
    location                = "${azurerm_resource_group.rg.location}"
    kind                    = "Linux"
    reserved                = true

    sku {
        tier = "Basic"
        size = "B1"
    }
}

resource "azurerm_app_service" "app" {
    name                = "app-${var.appservice_name}-${random_string.random.result}"
    resource_group_name = "${azurerm_resource_group.rg.name}"
    location            = "${azurerm_resource_group.rg.location}"
    app_service_plan_id = "${azurerm_app_service_plan.plan.id}"

    app_settings = {
        WEBSITES_ENABLE_APP_SERVICE_STORAGE = false
        API_VERSION = var.build_version
    }

    site_config {
        linux_fx_version        = "COMPOSE|${filebase64("docker-compose.yml")}"
        always_on               = "true"
    }
}