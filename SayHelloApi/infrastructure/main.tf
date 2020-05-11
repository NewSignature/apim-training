provider "azurerm" {
  version = "=2.1.0"
  features {}
}

// collect parameters
variable "app_name" {
    type  = "string"
}

// specify extras
resource "random_string" "random" {
  length    = 4
  upper     = false
  special   = false
}

// get data source references
resource "azurerm_resource_group" "rg" {
    name      = "rg-${var.app_name}"
    location  = "East US"
}

// specify resources
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

resource "azurerm_storage_account" "storage" {
    name                     = "st${var.app_name}${random_string.random.result}"
    resource_group_name      = "${azurerm_resource_group.rg.name}"
    location                 = "${azurerm_resource_group.rg.location}"
    account_tier             = "Standard"
    account_replication_type = "LRS"
}

resource "azurerm_function_app" "funcApp" {
    name                       = "func-${var.app_name}-${random_string.random.result}"
    location                   = "${azurerm_resource_group.rg.location}"
    resource_group_name        = "${azurerm_resource_group.rg.name}"
    app_service_plan_id        = "${azurerm_app_service_plan.plan.id}"
    storage_connection_string  = "${azurerm_storage_account.storage.primary_connection_string}"
    version                    = "~3"

    app_settings = {
        FUNCTION_APP_EDIT_MODE                    = "readOnly"
        https_only                                = true
        WEBSITES_ENABLE_APP_SERVICE_STORAGE       = false
    }

    site_config {
      always_on         = true
      linux_fx_version  = "DOCKER|xximjasonxx/sayhello:v2.1"
    }
}
