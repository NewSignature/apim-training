provider "azurerm" {
  version = "=2.0.0"
  features {}
}

// collect parameters
variable "app_name" {
    type    = "string"
}

// specify extras
resource "random_string" "random" {
  length    = 4
  special   = false
  upper     = false
}

// configure data sources
resource "azurerm_resource_group" "rg" {
    name        = "rg-${var.app_name}"
    location    = "East US"
}

// specify resources to create
resource "azurerm_app_service_plan" "plan" {
    name                    = "plan-${var.app_name}-${random_string.random.result}"
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
    name                = "app-${var.app_name}-${random_string.random.result}"
    resource_group_name = "${azurerm_resource_group.rg.name}"
    location            = "${azurerm_resource_group.rg.location}"
    app_service_plan_id = "${azurerm_app_service_plan.plan.id}"
    site_config {
        linux_fx_version        = "DOCKER|xximjasonxx/weatherapi-basic:v2.1"
        always_on               = "true"
    }
}