provider "azurerm" {
  version = "=2.1.0"
  features {}
}

variable "apim_name" {
  type = string
  description = "Name of your APIM resource, no prefix"
  default = "my-training"
}

resource "random_string" "random" {
    length = 4
    upper = false
    special = false
}

# create the resource group
resource "azurerm_resource_group" "rg" {
    name        = "rg-${var.apim_name}-${random_string.random.result}"
    location    = "East US"

    tags = {
        projectName = "apim-training"
    }
}

# create the APIM instance
resource "azurerm_api_management" "apim" {
    name                = "apim-${var.apim_name}-${random_string.random.result}"
    resource_group_name = azurerm_resource_group.rg.name
    location            = azurerm_resource_group.rg.location
    publisher_name      = "Farrellsoft"
    publisher_email     = "admin@farrellsoft.dev"

    sku_name            = "Developer_1"
}

# define group for product access
resource "azurerm_api_management_group" "people_group" {
    name                = "people-group"
    resource_group_name = azurerm_resource_group.rg.name
    api_management_name = azurerm_api_management.apim.name
    display_name        = "PeopleGroup"
    description         = "User have access to People information APIs"
}

# create Products
resource "azurerm_api_management_product" "apim_product_std" {
    product_id              = "${var.apim_name}-people-product-std"
    api_management_name     = azurerm_api_management.apim.name
    resource_group_name     = azurerm_resource_group.rg.name

    display_name            = "PeopleStd"
    subscription_required   = true
    approval_required       = false
    published               = true
}

resource "azurerm_api_management_product" "apim_product_unl" {
    product_id              = "${var.apim_name}-people-product-unl"
    api_management_name     = azurerm_api_management.apim.name
    resource_group_name     = azurerm_resource_group.rg.name

    display_name            = "PeopleUnl"
    subscription_required   = true
    subscriptions_limit     = 10
    approval_required       = true
    published               = true
}

# associate products to relevant group
resource "azurerm_api_management_product_group" "people_group_people_std_product" {
    product_id          = azurerm_api_management_product.apim_product_std.product_id
    group_name          = azurerm_api_management_group.people_group.name
    api_management_name = azurerm_api_management.apim.name
    resource_group_name = azurerm_resource_group.rg.name
}

resource "azurerm_api_management_product_group" "people_group_people_unl_product" {
    product_id          = azurerm_api_management_product.apim_product_unl.product_id
    group_name          = azurerm_api_management_group.people_group.name
    api_management_name = azurerm_api_management.apim.name
    resource_group_name = azurerm_resource_group.rg.name
}

# create rate limiting policy for standard people product
resource "azurerm_api_management_product_policy" "people_product_std_policy" {
    product_id          = azurerm_api_management_product.apim_product_std.product_id
    api_management_name = azurerm_api_management.apim.name
    resource_group_name = azurerm_resource_group.rg.name

    xml_content = <<XML
<policies>
    <inbound>
        <rate-limit calls="10" renewal-period="60" />
    </inbound>
</policies>
XML
}

# create storage for terraform state
resource "azurerm_storage_account" "storage" {
    name                        = "storage${replace(var.apim_name, "-", "")}${random_string.random.result}"
    resource_group_name         = azurerm_resource_group.rg.name
    location                    = azurerm_resource_group.rg.location
    account_kind                = "BlobStorage"
    account_tier                = "Standard"
    account_replication_type    = "LRS"
}

# create the container for the remote state
resource "azurerm_storage_container" "state_container" {
    name                    = "tfstate"
    storage_account_name    = azurerm_storage_account.storage.name
    container_access_type   = "private"
}