import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  { path: 'account', canActivate: [AuthGuard], loadChildren: () => import('./modules/account/account.module').then(m => m.AccountModule) },
  { path: '', canActivate: [AuthGuard], loadChildren: () => import('./modules/main/main.module').then(m => m.MainModule) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
