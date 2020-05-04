
provider "azurerm" {
  version = "=2.0.0"
  features {}
}

// collect parameters
variable "resource_group_name" {
    type    = "string"
}

variable "appservice_base_name" {
    type    = "string"
}

// specify extras
resource "random_string" "random" {
  length    = 4
  upper     = false
  special   = false
}

// configure data sources
data "azurerm_resource_group" "rg" {
    name    = "${var.resource_group_name}"
}

// specify resources to create
resource "azurerm_app_service_plan" "plan" {
    name                    = "plan-${var.appservice_base_name}-${random_string.random.result}"
    resource_group_name     = "${data.azurerm_resource_group.rg.name}"
    location                = "${data.azurerm_resource_group.rg.location}"
    kind                    = "Linux"
    reserved                = true

    sku {
        tier = "Basic"
        size = "B1"
    }
}

resource "azurerm_app_service" "app" {
    name                = "app-${var.appservice_base_name}-${random_string.random.result}"
    resource_group_name = "${data.azurerm_resource_group.rg.name}"
    location            = "${data.azurerm_resource_group.rg.location}"
    app_service_plan_id = "${azurerm_app_service_plan.plan.id}"

    app_settings = {
        "WEBSITES_ENABLE_APP_SERVICE_STORAGE" = "false"
    }

    site_config {
        linux_fx_version        = "COMPOSE|${filebase64("docker-compose.yml")}"
        always_on               = "true"
    }
}