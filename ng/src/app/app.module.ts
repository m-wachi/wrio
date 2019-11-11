import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MaintblComponent } from './maintbl/maintbl.component';
import { MessagesComponent } from './messages/messages.component';
import { Temp01Component } from './temp01/temp01.component';

@NgModule({
  declarations: [
    AppComponent,
    MaintblComponent,
    MessagesComponent,
    Temp01Component
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
