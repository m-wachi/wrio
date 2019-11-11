import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MaintblComponent } from './maintbl/maintbl.component';
import { Temp01Component } from './temp01/temp01.component';

const routes: Routes = [
  { path: 'maintbl', component: MaintblComponent  },
  { path: 'temp01', component: Temp01Component }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
