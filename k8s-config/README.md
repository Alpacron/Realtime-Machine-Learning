### Creating the environment
```
microk8s kubectl create namespace rml
```

### To update the cluster

```commandline
git clone https://github.com/Alpacron/Realtime-Machine-Learning repo
microk8s kubectl create -f repo/k8s-config -n rml
```

### To clean up the cluster and remove the repo
```commandline
microk8s kubectl delete -f repo/k8s-config -n rml
microk8s kubectl delete namespace rml
```