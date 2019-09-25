#!/usr/bin/env bash
docker build -f TaxonomyClient/Dockerfile -t txclient .
docker build -f TaxonomyService/Dockerfile -t tti/ttf/taxonomyservice .
docker build -f envoy/Dockerfile -t tti/ttf/envoy .
docker build -f ../TTF-Web-UI/Dockerfile -t tti/ttf/ui ../TTF-Web-UI
docker build -f ../TTF-Printer -t tti/ttf/printer ../TTF-Printer
docker-compose up
