import { Component} from '@angular/core';
import { CommonModule} from '@angular/common';
import {  Chats, UserModel, Users } from '../../app.component';
import { HttpClient } from '@angular/common/http';
import { ChatModel } from '../../Models/chat.model';
import * as  signalR from '@microsoft/signalr';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
    users:UserModel[] = [];
    chats:ChatModel[] = [];
    selectedUserId: string = "";
    selectedUser: UserModel = new UserModel();
    User = new UserModel;
    hub: signalR.HubConnection | undefined;
    Message:string="";

  
    changeUser(user: UserModel){
      this.selectedUserId = user.id;
      this.selectedUser = user;
  
      this.http.get("https://localhost:7230/api/Chats/GetChats?userId="+this.User.id+"&toUserId="+this.selectedUserId).subscribe((res:any) => {
        this.chats = res
      });

      this.chats = Chats.filter(p=> p.toUserId == user.id && p.userId == "0" || p.userId == user.id && p.toUserId == "0");
    }

    constructor(
      private http:HttpClient
    ){
      this.User = JSON.parse(localStorage.getItem("accessToken") ?? "") 
      this.getUsers();

      this.hub = new signalR.HubConnectionBuilder().withUrl("https://localhost:7230/chat-hub").build();

      this.hub.start().then(()=>{
        console.log("Bağlantı açıldı...");
        this.hub?.invoke("Connect", this.User.id);
        this.hub?.on("Users",(res:UserModel) => {
          console.log(res);
          const user = this.users.find(p=>p.id == res.id)!.status=res.status;
        });

        this.hub?.on("Messages",(res:ChatModel) =>{
          console.log(res);
          if (this.selectedUserId == res.userId) {
            this.chats.push(res);
          }
        } );
      })
    }

    getUsers(){
      this.http.get<UserModel[]>("https://localhost:7230/api/Chats/GetUsers").subscribe(res => {
        this.users = res.filter(p=> p.id != this.User.id);
      })
    }
    logout(){
      localStorage.clear();
      document.location.reload();
    }

    SendMessage(){
      const data ={
        "UserId":this.User.id,
        "ToUserId":this.selectedUserId,
        "Message":this.Message
      }
      this.http.post<ChatModel>("https://localhost:7230/api/Chats/SendMessage",data).subscribe(res=>{
        this.chats.push(res);
        this.Message="";
      });
    }
  
  }
  

