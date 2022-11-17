### Setting up the cluster for the first time
```
microk8s kubectl create namespace rml
git clone https://github.com/Alpacron/Realtime-Machine-Learning repo
microk8s kubectl create secret generic global-secret --from-file=appsettings.secrets.json -n rml
microk8s kubectl apply -f repo/k8s-config
```

### To update the cluster

```commandline
microk8s kubectl delete -f repo/k8s-config
rm -rf repo
git clone https://github.com/Alpacron/Realtime-Machine-Learning repo
microk8s kubectl apply -f repo/k8s-config
```

### To clean up the cluster and remove the repo
```commandline
microk8s kubectl delete -f repo/k8s-config
microk8s kubectl delete namespace rml
rm -rf repo
```