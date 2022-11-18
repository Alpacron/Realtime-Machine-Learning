# Kubernetes server setup

### Initial set-up microk8s on netlab with k8s template
```commandline
sudo snap refresh microk8s --channel=1.25/stable
microk8s enable dns
microk8s enable ingress
```

Optionally:
```commandline
microk8s enable community
microk8s enable portainer
```

### Setting up the cluster for the first time
To setup the cluster first run:
```commandline
microk8s kubectl create namespace rml ; git clone https://github.com/Alpacron/Realtime-Machine-Learning repo
```

Then enter some secrets in the secrets file:
```commandline
nano repo/kubernetes/appsettings.secrets.json
```

Now we can apply the secrets and config files:
```commandline
microk8s kubectl create secret generic global-secret --from-file=repo/kubernetes/appsettings.secrets.json -n rml\
&& microk8s kubectl apply -f repo/kubernetes/config
```

### Updating the cluster
First delete the old files:
```commandline
microk8s kubectl delete -f repo/kubernetes/config && rm -rf repo
```

Then apply the latest version:
```commandline
git clone https://github.com/Alpacron/Realtime-Machine-Learning repo && microk8s kubectl apply -f repo/kubernetes/config
```

### To clean up the cluster
Delete repo and namespace:
```commandline
rm -rf repo ; microk8s kubectl delete namespace rml
```