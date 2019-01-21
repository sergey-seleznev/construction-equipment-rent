# ConstructionEquipmentRent

This repository contains my solution to a software developer position home assignment at [Bondora](https://www.bondora.com/en).

## Solution layout

The solution consists of 3 projects and a folder of deployment setup:

* **ConstructionEquipmentRent.API**  
  ASP<span></span>.NET Core WebAPI-powered backend

* **ConstructionEquipmentRent.API.Tests**  
  .NET Core library containing XUnit unit tests for the most important API code

* **ConstructionEquipmentRent<span></span>.Web**  
  ASP<span></span>.NET Core Web application hosting single-page application frontend

* **k8s**  
  Configuration files and script automating solution deployment onto a local minikube Kubernetes cluster 

## Component communication

Frontend is implemented as a pretty much static single web page, querying all the actual data from API endpoint directly.  
That helps to offload frontend application, that only needs to give away static resources in this approach.  
Due to that fact, there are no direct calls from web server to the API. API is only queried from client-side browser JavaScript.  
So the communication flow looks like that: `[Web server] <=> [Web client] <=> [API server]`.  
Interprocess communication is employing REST HTTP protocol, that is a de-facto modern scalable application standard.

## Running

* **Unit-tests**  
  Visual Studio 2017 and Visual Studio 2019 Preview automatically detect existing unit tests after solution build.  
  The IDE and Resharper should see 11 quickly passing tests (parallel execution supported).

* **Local execution**  
  The solution is configured for multiple startup projects, running both backend and frontend together.  
  So the simplest way to run it is to click the Start button from Visual Studio's debugger panel.  
  Frontend page opens automatically in the default browser.

* **Kubernetes cluster execution**  
  The solution features an option to be deployed and executed in a scalable Kubernetes environment.  
  You can use a lightweight local Kubernetes installation to try that out: 
  * ensure `minikube` is installed, up and running,
  * run the following PowerShell commands:
    ```
    cd k8s
    ./deploy.ps1
    ```
  The deployment script creates all the resources required to test the application execution in a Kubernetes cluster.  
  Since API server is accessed from client-side, it has to be exposed externally in addition to frontend.  
  Upon completion, it also opens the frontend UI and Kubernetes dashboard in the browser.  

## Limitations and further improvements

* From the functional point of view, only the mentioned features were implemented.  
  For example, cart item removal is not possible at the moment (as it hasn't been requested).

* API tests cover only the most important logic. Those can be expanded to validate proper calls from controllers etc.

* Visual Studio reports NU1603 warning related to inconsistent internally dependent package versions.  
  Don't know if we can resolve that. Hope they harmonize the SDK dependencies some day.

* Proper code change versioning should be used instead of just publishing the whole codebase.

* Frontend code needs strong refactoring &mdash; grouping the JavaScript methods, connecting them to visual elements properly.

* Hosting a static page with an ASP.<span></span>NET Core application looks way too excessive.  
  Being not limited in options, I would rather employ a more lightweight server.

* Frontend UI, as well as invoice content look and feel might be improved a lot.

* Minikube deployment automated with a PowerShell script restricts the executing environment to Windows.  
  That can be easily ported to bash script.

* Normally Kubernetes configuration would bind services to proper DNS hostnames rather than dynamically assigned IPs.  
  That would eliminate the need to retrieve and pass the non-static API endpoint URL into the Web deployment.

* Kubernetes deployment offers excellent scaling opportunities to the backend service.  
  However, in order to achieve it, we have to implement proper shared data storage (i.e. database).  
  Otherwise consequent requests could arrive onto different instances, that doesn't share in-memory storage content.  
  So that an order is created on one node, and then asked to be populated with items on another one.

## Time tracking

* API backend &mdash; 1,5h
* API unit tests &mdash; 1h
* Web frontend &mdash; 2,5h
* Containerization &mdash; 1h
* K8s deployment &mdash; 1,5h
* Code publication &mdash; 0,5h
