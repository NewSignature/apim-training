
provider "azurerm" {
  version = "=2.8.0"
  features {}
}

terraform {
    backend "azurerm" {
        # filled in by azure devops runner
    }
}

# collect variables
variable "rg_name" {
  type = string
}

variable "apim_name" {
  type = string
}

variable "app_name" {
  type = string
}

variable "app_service_base_url" {
  type = string
}

# resolve the resource group
data "azurerm_resource_group" "rg" {
  name = var.rg_name
}

# resolve the products
data "azurerm_api_management_product" "apim_product_std" {
    product_id          = "${var.app_name}-people-product-std"
    api_management_name = var.apim_name
    resource_group_name = data.azurerm_resource_group.rg.name
}

data "azurerm_api_management_product" "apim_product_unl" {
    product_id              = "${var.app_name}-people-product-unl"
    api_management_name     = var.apim_name
    resource_group_name     = data.azurerm_resource_group.rg.name
}

# get the resource that is our target API
resource "azurerm_api_management_api" "apim_people_api" {
  name                = "people-api"
  resource_group_name = data.azurerm_resource_group.rg.name
  api_management_name = var.apim_name

  revision            = 1
  display_name        = "People API"
  path                = "people"
  protocols           = [ "https" ]
  service_url         = var.app_service_base_url
}

# add the api to our products
resource "azurerm_api_management_product_api" "product-std-api" {
  api_name            = azurerm_api_management_api.apim_people_api.name
  product_id          = data.azurerm_api_management_product.apim_product_std.product_id
  resource_group_name = data.azurerm_resource_group.rg.name
  api_management_name = var.apim_name
}

resource "azurerm_api_management_product_api" "product-unl-api" {
  api_name            = azurerm_api_management_api.apim_people_api.name
  product_id          = data.azurerm_api_management_product.apim_product_unl.product_id
  resource_group_name = data.azurerm_resource_group.rg.name
  api_management_name = var.apim_name
}