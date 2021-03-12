import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MaintblComponent } from './maintbl/maintbl.component';
import { Temp01Component } from './temp01/temp01.component';
import { Temp02Component } from './temp02/temp02.component';
import { Temp03Component } from './temp03/temp03.component';
import { Temp04Component } from './temp04/temp04.component';

const routes: Routes = [
  { path: 'maintbl', component: MaintblComponent  },
  { path: 'temp01', component: Temp01Component },
  { path: 'temp02', component: Temp02Component },
  { path: 'temp03', component: Temp03Component },
  { path: 'temp04', component: Temp04Component }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
