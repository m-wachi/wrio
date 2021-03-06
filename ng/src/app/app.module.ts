import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MaintblComponent } from './maintbl/maintbl.component';
import { MessagesComponent } from './messages/messages.component';
import { Temp01Component } from './temp01/temp01.component';
import { Temp02Component } from './temp02/temp02.component';
import { Temp03Component } from './temp03/temp03.component';
import { Temp04Component } from './temp04/temp04.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    MaintblComponent,
    MessagesComponent,
    Temp01Component,
    Temp02Component,
    Temp03Component,
    Temp04Component
  ],
  imports: [
    CommonModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatIconModule,
    MatChipsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
