#!/usr/bin/env bash
docker build  -f TaxonomyClient/Dockerfile --rm -t tti/ttf/taxonomyclient:1.0 .
docker build  -f TaxonomyHost/Dockerfile --rm -t tti/ttf/taxonomyservice:1.0 .

