import { Component, Input, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Message } from 'src/app/_models/message';
import { tap } from 'rxjs/operators';
import { MessageService } from 'src/app/_services/message.service';
import { Ad } from 'src/app/_models/ad';

@Component({
  selector: 'app-ad-messages',
  templateUrl: './ad-messages.component.html',
  styleUrls: ['./ad-messages.component.css']
})
export class AdMessagesComponent implements OnInit {
  @Input() recipientId: number;
  @Input() ad: Ad;

  messages: Message[];
  newMessage: any;
  textareaPlaceholderText = '';

  constructor(private messageService: MessageService,
              private authService: AuthService,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.newMessage = {
      adId: this.ad.id,
      senderId: this.getLoggedInUserId(),
      recipientId: this.recipientId,
      content: ''
    };
  }

  loadMessages() {
    this.messageService.getMessageThread(this.ad.id, this.recipientId)
      .pipe(
        tap(messages => {
          for (const message of messages) {
            if (message.recipientId === this.getLoggedInUserId() && message.isRead === false) {
              this.messageService.markMessageAsRead(message.id).subscribe(() => {
                message.isRead = true;
                MessageService.unreadMessagesCount--;
              }, error => {
                this.alertify.error(error);
              });
            }
          }
        })
      )
      .subscribe(result => {
        this.messages = result;
        this.setTextareaPlaceholder();
      }, error => {
        this.alertify.error(error);
      });
  }

  setTextareaPlaceholder() {
    this.textareaPlaceholderText = 'Въведете вашето съобщение';
    if (this.recipientId === this.ad.userId) { // recipient is the ad owner
      this.textareaPlaceholderText += ' до ' + this.ad.userName;
    } else { // recipient is other user with id = this.recipientId
      this.messages.some(m => {
        if (m.senderId === this.recipientId && m.recipientId === this.getLoggedInUserId()) {
          this.textareaPlaceholderText += ' до ' + m.senderUsername;
        } else {
          this.textareaPlaceholderText += ' до ' + m.recipientUsername;
        }
        return true;
      });
    }
  }

  getLoggedInUserId() {
    return +this.authService.decodedToken.nameid;
  }

  sendMessage() {
    this.newMessage.content = this.newMessage.content.trim();
    if (this.newMessage.content === '') {
      this.alertify.error('Съобщението не може да бъде празно');
      return;
    }

    const messageLength = this.newMessage.content.length;
    if (isNaN(messageLength) || messageLength < 2) {
      this.alertify.error('Съобщението трябва да бъде поне 2 символа');
      return;
    }

    this.messageService.sendMessage(this.newMessage).subscribe(sentOn => {
      const message: any = {
        senderId: this.newMessage.senderId,
        senderUsername: this.authService.decodedToken.unique_name,
        senderDeleted: false,
        content: this.newMessage.content,
        sentOn,
        isRead: false,
      };
      this.messages.push(message);
      this.newMessage.content = '';
    }, error => {
      this.alertify.error(error);
    });
  }
}
