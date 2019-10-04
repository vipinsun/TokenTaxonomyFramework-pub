#!/usr/bin/env bash
docker build -f TaxonomyClient/Dockerfile -t txclient:1.0 .
docker build -f TaxonomyService/Dockerfile -t ttfregistry.azurecr.io/tti/ttf/taxonomyservice:1.0 .
docker build -f envoy/Dockerfile -t ttfregistry.azurecr.io/tti/ttf/envoy:1.0 .
docker build -f ../TTF-Web-UI/Dockerfile -t ttfregistry.azurecr.io/tti/ttf/ui:1.0 ../TTF-Web-UI
docker build -f ../TTF-Printer/Dockerfile -t ttfregistry.azurecr.io/tti/ttf/printer:1.0 ../TTF-Printer


docker push ttfregistry.azurecr.io/tti/ttf/taxonomyservice:1.0
docker push ttfregistry.azurecr.io/tti/ttf/envoy:1.0
docker push ttfregistry.azurecr.io/tti/ttf/ui:1.0
