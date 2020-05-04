provider "azurerm" {
  version = "=2.1.0"
  features {}
}

// collect parameters
variable "resource_group_name" {
    type    = "string"
}

variable "func_base_name" {
    type    = "string"
}

// specify extras
resource "random_string" "random" {
  length    = 4
  upper     = false
  special   = false
}

// get data source references
data "azurerm_resource_group" "rg" {
    name = "${var.resource_group_name}"
}

// specify resources
resource "azurerm_app_service_plan" "plan" {
  name                    = "plan-${var.func_base_name}-${random_string.random.result}"
  resource_group_name     = "${data.azurerm_resource_group.rg.name}"
  location                = "${data.azurerm_resource_group.rg.location}"
  kind                    = "Linux"
  reserved                = true

  sku {
    tier = "Basic"
    size = "B1"
  }
}

resource "azurerm_storage_account" "storage" {
    name                     = "st${var.func_base_name}${random_string.random.result}"
    resource_group_name      = "${data.azurerm_resource_group.rg.name}"
    location                 = "${data.azurerm_resource_group.rg.location}"
    account_tier             = "Standard"
    account_replication_type = "LRS"
}

resource "azurerm_function_app" "funcApp" {
    name                       = "func-${var.func_base_name}-${random_string.random.result}"
    location                   = "${data.azurerm_resource_group.rg.location}"
    resource_group_name        = "${data.azurerm_resource_group.rg.name}"
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
      linux_fx_version  = "DOCKER|xximjasonxx/sayhello:v2"
    }
}
