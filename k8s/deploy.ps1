$app = "construction-equipment-rent"

# use minikube's docker environment
minikube docker-env | Invoke-Expression

# run local docker registry
docker run -d -p 5000:5000 --restart=always --name registry registry:2
$reg = "localhost:5000"

# build images
docker build -t $reg/$app.api "../ConstructionEquipmentRent.API"
docker build -t $reg/$app.web "../ConstructionEquipmentRent.Web"

# push images to local repository
docker push $reg/$app.api
docker push $reg/$app.web

# deploy api
kubectl create -f "deploy-api.yml"

# get api external endpoint
$apiBaseUrl = minikube service $app-api --url

# prepare web deployment:
# put the actual api endpoint into environment variable
$webDeployTemplate = Get-Content "deploy-web.yml" -Raw
$webDeploy = [System.IO.Path]::GetTempFileName()
$ExecutionContext.InvokeCommand.ExpandString($webDeployTemplate) | `
  Out-File $webDeploy -Encoding "UTF8"

# deploy web
kubectl create -f $webDeploy

# launch web
minikube service $app-web

# launch k8s dashboard
minikube dashboard
