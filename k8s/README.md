<h1 align="center">K8S - ANTIVALOR</h1>
<h4 align="center"><strong>Semester 6 Individual - Aron Hemmes</strong></h4>
<p align="center">Setup of the kubernetes environment on a ubuntu server.</p>
<br><br>

## Implementation
The kubernets cluster was deployed on a VM in the vshpere environment.

The first step was to setup microk8s environment and enable different addons such as dns, ingress, portainer and observability.

Portainer was used a way to monitor and see logs of the containers. On portainer Helm was used to setup different services including Galera MariaDB and RabbitMQ. Galera MariaDB is a scalable cloud storage which automatically scales up/down the database depending on load. Observability creates a grafana dashboard which can be used to see metrics and load on the containers.

## Setup

### Setup micok8s
```commandline
sudo snap refresh microk8s --channel=1.25/stable
microk8s enable dns
microk8s enable ingress
```

Install portainer and observability (optional):
```commandline
microk8s enable community
microk8s enable portainer
microk8s enable observability
```

### Setup cluster
To setup the cluster first run:
```commandline
microk8s kubectl create namespace rml ; git clone https://github.com/Alpacron/antivalor repo
```

Then enter some secrets in the secrets file:
```commandline
nano repo/k8s/appsettings.secrets.json
```

Now we can apply the secrets and config files:
```commandline
microk8s kubectl create secret generic global-secret --from-file=repo/k8s/appsettings.secrets.json -n rml\
&& microk8s kubectl apply -f repo/k8s/config
```

### Update cluster
First delete the old files:
```commandline
microk8s kubectl delete -f repo/k8s/config && rm -rf repo
```

Then apply the latest version:
```commandline
git clone https://github.com/Alpacron/antivalor repo ; microk8s kubectl apply -f repo/k8s/config
```

### Clean up cluster
Delete repo and namespace:
```commandline
rm -rf repo ; microk8s kubectl delete namespace rml
```
